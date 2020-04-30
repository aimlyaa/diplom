from django.shortcuts import render
from rest_framework.response import Response
from rest_framework.views import APIView
from rest_framework.response import Response
from .models import User, Test, TestData, TestsResult

from random import choice
from string import ascii_uppercase

class LoginView(APIView):
    def post(self, request):
        group = request.data.get('group')
        name = request.data.get('name')
        print("Попытка входа: {}, {}".format(name, group))
        try:
            user = User.objects.get(name = name, group = group)
            new_token = ''.join(choice(ascii_uppercase) for i in range(8))
            user.token = new_token
            user.save()
            return Response({"token": new_token})
        except:
            return Response({"token": "False"})

class TestsListView(APIView):
    def get(self, request):
        request_token = request.headers['token']
        if User.objects.filter(token = request_token).count() > 0:
            user = User.objects.get(token = request_token)
            tests = Test.objects.filter(avaliable_groups__contains = user.group)
            return Response({"testsList": tests.values("name")})
        return Response({"testsList": "nope"})

class TestDataView(APIView):
    def get(self, request):
        request_token = request.headers['token']
        test_name = request.GET['test']
        if User.objects.filter(token = request_token).count() > 0:
            test = Test.objects.get(name = test_name)
            test_data = TestData.objects.filter(meta = test)
            user = User.objects.get(token = request_token)
            if TestsResult.objects.filter(user = user, test = test).count() == 0:
                test_answers = ''.join(',3' for i in test_data.values("question"))
                new_test_result = TestsResult(user = user, test = test, ended = 0, answers = test_answers[1:], av_time=test.avaliable_time)
                new_test_result.save()
            elif TestsResult.objects.get(user = user, test = test).ended == 1:
                return Response({"answers": TestsResult.objects.get(user = user, test = test).answers, "ended": 1})              
            time_exp = TestsResult.objects.get(user = user, test = test).av_time
            return Response({"testData": test_data.values("question", "answer1", "answer2", "answer3", "answer4"),"avaliableTime":time_exp, "answers":TestsResult.objects.get(user = user, test = test).answers})
        return Response({"test": "nope"})

class CheckAnswerView(APIView):
    def post(self, request):
        request_token = request.headers['token']
        test_name = request.data.get('testName')
        question_string = request.data.get('questionString')
        answer_index = request.data.get('answerIndex')
        if User.objects.filter(token = request_token).count() > 0:
            test_meta = Test.objects.get(name = test_name)

            test = TestData.objects.filter(meta = test_meta, question = question_string)
            user = User.objects.get(token = request_token)
            tst_name = Test.objects.get(name = test_name)
            end_test_data = TestsResult.objects.get(user = user, test = tst_name)
            answers = list(map(str, end_test_data.answers.split(',')))
            question_index = -1
            for x in TestData.objects.filter(meta = tst_name):
                question_index += 1
                if x.question == test[0].question:
                    print("finded")
                    break
            print(question_index)
            if( test.values("answer" + str(answer_index + 1))[0]['answer' + str(answer_index + 1)] == test.values("good_answer")[0]['good_answer'] ):
                answers[question_index] = '1'
                end_test_data.answers = ','.join(map(str, answers))
                end_test_data.save()
                return Response({"result": "1"})
            else:
                answers[question_index] = '0'
                end_test_data.answers = ','.join(map(str, answers))
                end_test_data.save()
                return Response({"result": "0"})
        return Response({"result": "nope"})

class EndTestView(APIView):
    def post(self, request):
        request_token = request.headers['token']
        test_name = request.data.get('testName')
        if User.objects.filter(token = request_token).count() > 0:
            user = User.objects.get(token = request_token)
            test = Test.objects.get(name = test_name)
            end_test_data = TestsResult.objects.get(user = user, test = test)
            end_test_data.ended = 1
            end_test_data.save()
            return Response({"result": "ok"})
        return Response({"result": "nope"})

class SyncTimeView(APIView):
    def post(self, request):
        request_token = request.headers['token']
        test_name = request.data.get('testName')
        if User.objects.filter(token = request_token).count() > 0:
            user = User.objects.get(token = request_token)
            test = Test.objects.get(name = test_name)
            current_test = TestsResult.objects.get(user = user, test = test)
            current_test.av_time -= 1
            current_test.save()
            return Response({"result": "ok"})
        return Response({"result": "nope"})
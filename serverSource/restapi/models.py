from django.db import models

class User(models.Model):
    name = models.CharField(max_length=255)
    group = models.CharField(max_length=25)
    token = models.CharField(max_length=8)
    def __str__(self):
        return self.name + ', ' + self.group

class Test(models.Model):
    name = models.CharField(max_length=255)
    avaliable_groups = models.CharField(max_length=255)
    avaliable_time = models.IntegerField()
    def __str__(self):
        return self.name

class TestData(models.Model):
    meta = models.ForeignKey(Test, on_delete=models.CASCADE)
    question = models.CharField(max_length=255)
    answer1 = models.CharField(max_length=255)
    answer2 = models.CharField(max_length=255)
    answer3 = models.CharField(max_length=255)
    answer4 = models.CharField(max_length=255)
    good_answer = models.CharField(max_length=255)
    def __str__(self):
        return self.question
    @property
    def get_answers_list(self):
        return "{},{},{},{}".format(self.answer1, self.answer2, self.answer3, self.answer4)

class TestsResult(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)
    test = models.ForeignKey(Test, on_delete=models.CASCADE)
    answers = models.CharField(max_length=255)
    av_time = models.IntegerField()
    ended = models.IntegerField()
    def __str__(self):
        return self.user.name + ' ' + self.user.group + ', ' + self.test.name
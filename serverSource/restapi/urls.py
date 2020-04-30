from django.urls import path
from .views import LoginView, TestsListView, TestDataView, EndTestView, CheckAnswerView, SyncTimeView
app_name = "restapi"
urlpatterns = [
    path('auth/', LoginView.as_view()),
    path('tests_list/', TestsListView.as_view()),
    path('get_test/', TestDataView.as_view()),
    path('send_answer/', CheckAnswerView.as_view()),
    path('end_test/', EndTestView.as_view()),
    path('sync_time/', SyncTimeView.as_view()),
]
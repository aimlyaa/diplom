from django.contrib import admin
from .models import User, Test, TestData, TestsResult

admin.site.register(User)
admin.site.register(Test)
admin.site.register(TestData)
admin.site.register(TestsResult)
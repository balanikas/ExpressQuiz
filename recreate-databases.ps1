set-executionpolicy remotesigned

Write-Host "ApplicationDbContextConfig"
Update-Database -ConfigurationTypeName ApplicationDbContextConfig -ProjectName ExpressQuiz.Core
Write-Host "QuizDbContextConfig"
Update-Database -ConfigurationTypeName QuizDbContextConfig -ProjectName ExpressQuiz.Core
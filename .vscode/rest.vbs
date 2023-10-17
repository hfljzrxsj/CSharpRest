Option Explicit
Dim objShell, objTimer, count, Counter, intResult, maxTime', objWMIService
Set objShell = CreateObject("WScript.Shell")
' Set objWMIService = GetObject("winmgmts:\\.\root\cimv2")
count = 1
maxTime=4
Sub RunREST(maxCount)
	objShell.Run "I:/html/rest.hta"
    ' 运行 git-bash.exe 的命令
    objShell.Run "D:/Git/git-bash.exe"' , 0, True
    
    count = count + 1
    
    If count <= maxCount Then
        ' 延迟 1分钟后再次执行 RunREST
        WScript.Sleep(60000)
        RunREST(maxCount)
    End If
	If count = maxCount+1 Then
       count = 1
    End If
End Sub
' 计数器初始值为40分钟
Counter = 40

' 循环执行，每隔1分钟减少计数器值
Do While True
    ' 等待1分钟
    WScript.Sleep(60000)

    ' 减少计数器值
    Counter = Counter - 1
    If maxTime = 0 Then
      RunREST(10)
      Counter = 40
      maxTime = 4
      ' continue
    ' 如果计数器值为0，弹出提醒窗口
    ElseIf Counter = 0 Then
        ' 弹出提醒窗口
        intResult = objShell.Popup("该休息一下了！", 0, "休息提醒", 1 + 48)

        ' 根据用户的点击结果执行相应的操作
        Select Case intResult
            Case 1 ' 点击“马上休息”
                ' 运行 rest.exe
                ' objShell.Run "I:/html/rest.hta"
                RunREST(5)
                ' 重置计数器为40分钟
                Counter = 40
                maxTime = 4
            Case 2 ' 点击“再看5分钟”
                ' 重置计数器为5分钟
                Counter = 5
                maxTime = maxTime - 1
        End Select
    End If
Loop

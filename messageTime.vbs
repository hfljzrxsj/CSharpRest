Set objShell = CreateObject("WScript.Shell")
intSeconds = 10
' 创建一个消息框
intResult = objShell.Popup("请在" & intSeconds & "秒内操作，否则将打开记事本。", intSeconds, "操作提醒", vbInformation + vbOKCancel)
' 检查用户的响应
If intResult = vbOK Then
    ' 用户点击了“确定”
    objShell.Popup "您点击了确定按钮。", 5, "操作提示", vbInformation
Else
    ' 用户没有及时响应，运行记事本
    objShell.Run "notepad.exe"
End If
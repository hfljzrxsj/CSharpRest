Option Explicit
Dim objShell, objTimer, count, Counter, intResult, maxTime', objWMIService
Set objShell = CreateObject("WScript.Shell")
' Set objWMIService = GetObject("winmgmts:\\.\root\cimv2")
count = 1
maxTime=4
Sub RunREST(maxCount)
	objShell.Run "I:/html/rest.hta"
    ' ���� git-bash.exe ������
    objShell.Run "D:/Git/git-bash.exe"' , 0, True
    
    count = count + 1
    
    If count <= maxCount Then
        ' �ӳ� 1���Ӻ��ٴ�ִ�� RunREST
        WScript.Sleep(60000)
        RunREST(maxCount)
    End If
	If count = maxCount+1 Then
       count = 1
    End If
End Sub
' ��������ʼֵΪ40����
Counter = 40

' ѭ��ִ�У�ÿ��1���Ӽ��ټ�����ֵ
Do While True
    ' �ȴ�1����
    WScript.Sleep(60000)

    ' ���ټ�����ֵ
    Counter = Counter - 1
    If maxTime = 0 Then
      RunREST(10)
      Counter = 40
      maxTime = 4
      ' continue
    ' ���������ֵΪ0���������Ѵ���
    ElseIf Counter = 0 Then
        ' �������Ѵ���
        intResult = objShell.Popup("����Ϣһ���ˣ�", 0, "��Ϣ����", 1 + 48)

        ' �����û��ĵ�����ִ����Ӧ�Ĳ���
        Select Case intResult
            Case 1 ' �����������Ϣ��
                ' ���� rest.exe
                ' objShell.Run "I:/html/rest.hta"
                RunREST(5)
                ' ���ü�����Ϊ40����
                Counter = 40
                maxTime = 4
            Case 2 ' ������ٿ�5���ӡ�
                ' ���ü�����Ϊ5����
                Counter = 5
                maxTime = maxTime - 1
        End Select
    End If
Loop

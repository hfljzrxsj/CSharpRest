Set objShell = CreateObject("WScript.Shell")
intSeconds = 10
' ����һ����Ϣ��
intResult = objShell.Popup("����" & intSeconds & "���ڲ��������򽫴򿪼��±���", intSeconds, "��������", vbInformation + vbOKCancel)
' ����û�����Ӧ
If intResult = vbOK Then
    ' �û�����ˡ�ȷ����
    objShell.Popup "�������ȷ����ť��", 5, "������ʾ", vbInformation
Else
    ' �û�û�м�ʱ��Ӧ�����м��±�
    objShell.Run "notepad.exe"
End If
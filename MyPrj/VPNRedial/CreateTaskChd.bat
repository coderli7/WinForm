@echo on



schtasks /create /tn  /f "VPN" /tr %cd%\VPNRedial.exe /SC ONLOGON 

::pause
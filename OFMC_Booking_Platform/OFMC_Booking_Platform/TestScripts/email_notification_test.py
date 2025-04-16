import requests
import json


import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

appointment_id = 1


url = f"https://localhost:7276/admin/cancelAppointment?appointmentId={appointment_id}"


response = requests.post(url, verify=False)  # the verification is set to False in order to workaround the authentication steps 


if response.status_code in [200, 302]:
    print(f"Cancellation was successful: Status {response.statu_code}")

else:
    print(f"Cancellation was NOT successful: Status {response.status_code}")

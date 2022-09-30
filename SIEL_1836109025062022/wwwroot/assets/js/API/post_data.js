    $(function(){
        $("#api").click(async function () {
            console.log("console_control");
            var myHeaders = new Headers();
            myHeaders.append("token", "EE7F349F-0F87-4A79-8607-04998F649C7B");
            myHeaders.append("Content-Type", "application/json");
            var raw = JSON.stringify({
                "institutionId": "5235225",
                "studentUserName": "test006",
                "studentFirstName": "Student",
                "studentLastName": "Test",
                "studentPassword": "12345",
                "studentEmail": "student05@test.com",
                "classId": "552250012"
            });
            var requestOptions = {
                method: 'POST',
                headers: myHeaders,
                body: raw,
                redirect: 'follow'
            };
            fetch("https://edapi.engdis.com/WebApi/External/InsertUser", requestOptions)
                .then(response => response.text())
                .then(result => console.log(result))
                .catch(error => console.log('error', error));
        });
                                    });


var fs = require('fs');
var request = require('request');
var sleep = require('system-sleep');


var id = 0
var splitLines = []
var valuesOnly = []
var peopleArr = []

const allFileContents = fs.readFileSync('Users.sql', 'utf-8');
allFileContents.split(/\r?\n/).forEach(line => {

    if (line.includes("insert into")) {

        let position = line.split(")insert into \"User\"")

        for (var i = 0; i < position.length; i++) {

            if (i % 2 == 0) {
                splitLines.push(position[i])
            } else {
                splitLines.push("insert into \"User\" " + position[i])
            }
        }
    }
});


for (var i = 0; i < splitLines.length; i++) {
    var splitStr = splitLines[i].split("values ")
    valuesOnly.push(splitStr[1])
}
splitLines = []

for (var i = 0; i < valuesOnly.length; i++) {
    var splitValues = valuesOnly[i].split(", ")

        var userName = ""
        for(var k = 1 ; k < splitValues[3].length ; k++){
            if(splitValues[3][k] == '\''){
                break;
            }else{
                userName += splitValues[3][k]
            }
        }   

        var email = ""
        for(var k = 2 ; k < splitValues[0].length ; k++){
            if(splitValues[0][k] == '\''){
                break;
            }else{
                email += splitValues[0][k]
            }
        }  


        var person = {
            "id": id,
            "username": userName,
            "pwd": "123",
            "email": email
        }

        id++
        peopleArr.push(person)
}



async function postPersion(person){
    var options = {
        agent: false, 
        method: 'POST',
        url: 'http://138.68.69.204:8081/simulator/register',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(person)
    };

    request(options, function (error, response, body) {
        if (error) throw new Error(error);
        console.log(body);
    });
}

async function peoplePost(){
    for (var i = 0; i < peopleArr.length; i++) {
        sleep(200);
        console.log(peopleArr[i])
        postPersion(peopleArr[i])
        
    }
}


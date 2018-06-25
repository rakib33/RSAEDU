$("#submit").click(function () {

    enableLoading();
try {

    var _ExamId = $("#ExamId").val();
    var _DeparmentId = $("#DeparmentId").val();
    var _MarkOption = $("#MarkOption").val();
    var _SubjectId = $("#Subject").val();
    var _FromRoll = $("#fromRoll").val();
    var _ToRoll = $("#toRoll").val();

    if (_ExamId != null && _ExamId != '' && _DeparmentId != null && _DeparmentId != '' && _SubjectId != null && _SubjectId != '' && _MarkOption != null && _MarkOption != '')
    {
        $.ajax({
            type: 'GET',
            url: '/ExamResult/Index',
            dataType: 'json',

            data: { ExamId: _ExamId, DeparmentId: _DeparmentId, Subject: _SubjectId, fromRoll: _FromRoll, toRoll: _ToRoll, MarkOption: _MarkOption, IsAjax: 1 },

            success: function (data) {

                console.log(data.msg);

                if (data.msg === 'ok') {

                    //display Loading gif
                    enableLoading();

                    createTable(data.result, data.type);

                    $("#bredcum").empty();
                    var th = document.createElement('label'); // Create Label.           
                    th.innerHTML = data.bredcum;
                    var div = document.getElementById('bredcum'); //Where want to create table
                    div.appendChild(th);

                   
                }
                else {
                    alert(data.msg);
                    disableLoading();
                }

            },
            error: function (ex) {
                alert('error: ' + ex);
                disableLoading();
            }
        });


        return false;
    }

   else if (_ExamId == null || _ExamId == '' || isNaN(_ExamId)) {
       alert('Please Select an Exam.');
       disableLoading();
       return false;
    }
   else if (_DeparmentId == null || _DeparmentId == '' || isNaN(_DeparmentId)) {
       
       alert('Please Select a Department.');
        disableLoading();
        return false;
    }
    else if (_SubjectId == null || _SubjectId == '' || isNaN(_SubjectId)) {
        alert('Please Select a Subject.');
        disableLoading();
        return false;
    }
    else if (_MarkOption === null || _MarkOption === '') {
        alert('Select a Mark Option!!');
        disableLoading();
        return false;
    }
    }catch(err){
        disableLoading();
                alert(err.message);
    }


    return false;
})


function enableLoading() {
    document.getElementById("load").style.display = "block";
    document.getElementById("submit").style.display = "none";
}

function disableLoading() {
    document.getElementById("load").style.display = "none";
    document.getElementById("submit").style.display = "block";
}

// ADD THE TABLE TO YOUR WEB PAGE.
function createTable(student,resultType) {

    arrHead = [];
    arrHead = ['Sl No', 'Student Id', 'Student Name', 'Roll No'];  // SIMPLY ADD OR REMOVE VALUES IN THE ARRAY FOR TABLE HEADERS.

    for (var i in resultType)
    {
        arrHead.push(resultType[i].ExamTypeName);
    }
    arrHead.push('Total');
    arrHead.push(''); //empty string for edit

    var empTable = document.createElement('table');
    empTable.setAttribute('id', 'attendence');            // SET THE TABLE ID.
    empTable.setAttribute('class', 'table table-striped table-bordered table-hover dt-responsive nowrap')


    var tr = empTable.insertRow(-1);
    //We know that Student has two column name and roll we get
    //and extra Sl No ,CheckBox get 4 field

    for (var h = 0; h < arrHead.length; h++) {
        var th = document.createElement('th'); // TABLE HEADER.           
        th.innerHTML = arrHead[h];
        tr.appendChild(th);
    }

    //display how many prototype

    $("#dtableAttendence").empty();

    var div = document.getElementById('dtableAttendence'); //Where want to create table
    div.appendChild(empTable);    // ADD THE TABLE TO YOUR WEB PAGE.

    //Add row

    $.each(student, function (i, _stu) {

        var empTab = document.getElementById('attendence');
        var rowCnt = empTab.rows.length;        // GET TABLE ROW COUNT.

        console.log('row Count :' + rowCnt);
        var tr = empTab.insertRow(rowCnt);      // TABLE ROW.

        var total = 0;
        var flag = 0;

        for (var c = 0; c < 5; c++) {

          
            if (c === 0)//Sl No
            {
                var td = document.createElement('td');
                td = tr.insertCell(c);
                var t = document.createTextNode("" + (i + 1));
                td.appendChild(t);
            }
            else if (c === 1) {

                var td = document.createElement('td');
                td = tr.insertCell(c);
                var t = document.createTextNode("" + _stu.StudentId);
                td.appendChild(t);
            }
            else if (c === 2) {

                var td = document.createElement('td');
                td = tr.insertCell(c);
                var t = document.createTextNode("" + _stu.StudentName);
                td.appendChild(t);
            }

            else if (c === 3) {

                  var td = document.createElement('td');
                  td = tr.insertCell(c);
                  var t = document.createTextNode("" + _stu.RollNo);
                  td.appendChild(t);
            }
        
            if (c > 3) {
                //print exam type result .and already one td is declared.
                total = 0;
                $.each(_stu.ResultType, function (i, _type) {
                
                    total += _type.MarksObtain;
                    var td = document.createElement('td');
                    td = tr.insertCell(c);
                    var t = document.createTextNode("" + _type.MarksObtain);
                    td.appendChild(t);

                    flag++;
                    
                })

                var td = document.createElement('td');
                td = tr.insertCell(c+flag);
                var t = document.createTextNode("" + total);
                td.appendChild(t);

                var td = document.createElement('td');
                td = tr.insertCell(c+flag+1);
                var th = document.createElement('span'); // TABLE HEADER.
                th.innerHTML = "<a href='/ExamResult/Edit?id=" + _stu.StudentId + "'>Edit</a>";  // '<a href="/ExamResult/Edit/">Edit</a>';
                td.appendChild(th);

            } //all loop complete

                  
        }
    });

    disableLoading();
}



$("#ExamId").change(function () {
    $("#Subject").empty();
});

$("#DeparmentId").change(function () {

    $("#Subject").empty();

    var depId = $("#DeparmentId").val();

    if (depId == '' || depId == null || isNaN(depId)) {

        alert('Please select a Department');
    } else {

        $.ajax({
            type: 'POST',
            url: '/ExamResult/LoadSubject', //'/Home/LoadSubject',

            dataType: 'json',

            data: { depId: $("#DeparmentId").val() },


            success: function (data) {

                console.log(data.msg);
                if (data.msg === 'Ok') {
                    //add initial option
                    $("#Subject").append('<option value="">Select Subject</option>');
                    $.each(data.list, function (i, state) {
                        $("#Subject").append('<option value="' + state.Id + '">' +
                             state.SubjectName + '</option>');
                        //alert('Success');

                    });
                }
            },
            error: function (ex) {
                alert('Failed to retrieve subject.' + ex);
            }
        });
    }
    return false;
})
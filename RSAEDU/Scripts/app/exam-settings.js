
$("#submit").click(function (e) {

    //alert('hited');


    var _ExamId = $("#ExamId").val();

    if (_ExamId == null || _ExamId == '' || isNaN(_ExamId))
        return false;
    else {

        $.ajax({

            type: 'GET',
            url: '/ExamSettings/Index',
            dataType:'json',
            data: {ExamId:_ExamId,IsAjax:1},
            success: function (data){
                console.log('success');
                if (data.msg === 'ok') {

                    $("#dtableExam").empty();
                    if (data.detail.length === 0) {
                        alert('Record Not Found !!');
                    } else {
                        createTable(data.detail);
                    }
                }
            },
            error: function (ex)
            {
                alert('error'+ex);
            }

    });
   

    }

    return false;
   // e.preventDefault();
});

function createTable(details){
    
    arrHead = [];
    arrHead = ['Sl No', 'Exam Name', 'Subject Name', 'Exam Type', 'Total Marks', 'Pass Mark',''];

    var table = document.createElement('table');
    table.setAttribute('id', 'examSettings');
    table.setAttribute('class', 'table table-striped table-bordered table-hover dt-responsive nowrap');
    //add table header row
    var tr = table.insertRow(-1);

    for (var h = 0; h < arrHead.length ; h++)
    {
        var th = document.createElement('th');
        th.innerHTML = arrHead[h];
        tr.appendChild(th);

    }
    $("#dtableExam").empty();

    var div = document.getElementById("dtableExam");
    div.appendChild(table);

    //add row
    $.each(details, function (i, _stu) {

        var tableRow = document.getElementById("examSettings");
        var RowCount = tableRow.rows.length;
        var tr = tableRow.insertRow(RowCount); //extend row index for add new row each time


        for (var c = 0; c < arrHead.length; c++) {

            var td = document.createElement('td');          // TABLE DEFINITION.
            td = tr.insertCell(c);
            if (c === 0)//Sl No
            {

                var t = document.createTextNode("" + (i + 1));
                td.appendChild(t);
            }

           else if (c === 1) {

               var t = document.createTextNode("" + _stu.ExamName);
                td.appendChild(t);
            }

           else if (c === 2) {
               var t = document.createTextNode("" + _stu.SubjectName);
                td.appendChild(t);
            }
           else if (c === 3) {
               var t = document.createTextNode("" + _stu.ExamType);
                td.appendChild(t);
            }
           else if (c === 4) {
               var t = document.createTextNode("" + _stu.TotalMarks);
               td.appendChild(t);
            }
           else if (c === 5) {
               var t = document.createTextNode("" + _stu.PassMarks);
               td.appendChild(t);
           }
           else if (c === 6) {
                var th = document.createElement('span'); // TABLE HEADER.
                th.innerHTML = "<a href='/ExamSettings/Edit?id=" + _stu.Id + "'>Edit</a>";  // '<a href="/ExamResult/Edit/">Edit</a>';
                td.appendChild(th);
            }

        }
    })
    
 
}
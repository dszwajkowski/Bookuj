var count = 1;

function addRow(tableID) {
    let table = document.getElementById(tableID);

    var index = table.rows.length - 1;
    var row = table.insertRow(index);
    row.setAttribute("class", "lodgingOptionsRow");

    var personCount = row.insertCell(0);
    var bedCount = row.insertCell(1);
    var roomCount = row.insertCell(2);
    var size = row.insertCell(3);
    var price = row.insertCell(4);
    var action = row.insertCell(5);

    personCount.innerHTML = "<input type='text' class='form-control' name='Command.LodgingOptions[" + (count - 1) +"].PersonCount' placeholder='Ilość osób' required='required'/>";
    bedCount.innerHTML = "<input type='text' class='form-control' name='Command.LodgingOptions[" + (count - 1) +"].BedCount' placeholder='Ilość łóżek' required='required'/>";
    roomCount.innerHTML = "<input type='text' class='form-control' name='Command.LodgingOptions[" + (count - 1) +"].RoomCount' placeholder='Ilość pokoi' required='required'/>";
    size.innerHTML = "<input type='text' class='form-control' name='Command.LodgingOptions[" + (count - 1) +"].Size' placeholder='Wielkość (m²)' required='required'/>";
    price.innerHTML = "<input type='text' class='form-control' name='Command.LodgingOptions[" + (count - 1) +"].Price' placeholder='Cena za noc' required='required'/>";
    action.innerHTML = "<button type='button' class='btn btn-danger' onclick = 'deleteRow2(" + index + ")'> -</button >";
    count++;
}

function deleteRow2(index) {
    let table = document.getElementById("lodgingOptions");
    let tr = document.querySelectorAll(".lodgingOptionsRow:nth-child(" + index + ") ~ *");
    var newIndex = index - 1;

    for (var i = 0; i < tr.length-1; i++) {;
        var td = tr[i].children;
        var personCount = td[0].children[0];
        var bedCount = td[1].children[0];
        var roomCount = td[2].children[0];
        var size = td[3].children[0];
        var price = td[4].children[0];
        var deleteButton = td[5].children[0];


        personCount.setAttribute("name", "Command.LodgingOptions[" + newIndex +"].PersonCount");
        bedCount.setAttribute("name", "Command.LodgingOptions[" + newIndex + "].BedCount");
        roomCount.setAttribute("name", "Command.LodgingOptions[" + newIndex + "].RoomCount");
        size.setAttribute("name", "Command.LodgingOptions[" + newIndex + "].Size");
        price.setAttribute("name", "Command.LodgingOptions[" + newIndex + "].Price");
        deleteButton.setAttribute("onclick", "deleteRow2(" + newIndex +")");
        //var test = td[0].childNodes[0];
        //var test2 = td[1].childNodes[0];
        //var test3 = td[2].childNodes[0];
        //test.setAttribute("name", "test");
        //test.nodeName
        //var testestset = inputs.childNodes;
        //var estest = inputs.children[0];
        //var childElements2 = childElements.children;
        //childElements[0].setAttribute("name", 'dddddasdsa' + index);
        newIndex++;
    }
    table.deleteRow(index);
    count--;
}
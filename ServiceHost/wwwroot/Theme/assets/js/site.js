function remove() {
    var selectElement = document.getElementById("MembershipType").value;
    var selectMembership1 = document.getElementById("selectMembership1");
    var selectMembership2 = document.getElementById("selectMembership2");

    if (selectElement == "1") {
        selectMembership1.classList.remove("hidden");
        selectMembership2.classList.add("hidden");
    }
    if (selectElement == "2") {
        selectMembership2.classList.remove("hidden");
        selectMembership1.classList.add("hidden");
    }
    if (selectElement == "0") {
      selectMembership2.classList.add("hidden");
      selectMembership1.classList.add("hidden");
    }

}

function FillUnis(lstUniTypes) {
    var lstUnis = $("#lstUnis");
    lstUnis.empty();
    var selectedType = lstUniTypes.options[lstUniTypes.selectedIndex].value;

    if (selectedType != null && selectedType != '') {
        $.getJSON('/Controllers/Uni/GetUnisByType', {typeId: selectedType}, function (unis) {
            if (unis != null && !jQuery.isEmptyObject(unis)) {
                $.each(unis, function (index, uni) {
                    lstUnis.append($('<option/>',
                        {
                            value: uni.value,
                            text: uni.text
                        }));
                });
            }
        });
    }

    return;
}

function FillActiveUnisS(lstUniTypes) {
    var lstUnis = $("#lstUnis");
    lstUnis.empty();

    var selectedType = lstUniTypes.value;


    lstUnis.append($('<option/>', { value: "0", text: "دانشگاه را انتخاب کنید" }));

    if (!selectedType || selectedType === "0") {
        return;
    }

    $.getJSON('/Controllers/ActiveUni/GetUnisByType', { typeId: selectedType })
        .done(function (unis) {
            lstUnis.empty();
            $.each(unis, function (index, uni) {
                lstUnis.append($('<option/>', {
                    value: uni.value,
                    text: uni.text
                }));
            });
        })
        .fail(function (xhr) {
            alert('خطا در دریافت لیست دانشگاه‌ها');
        });
}


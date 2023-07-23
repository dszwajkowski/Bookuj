document.querySelectorAll('.addToCart').forEach((elem) => {
    elem.addEventListener('click', async (e) => {
        e.preventDefault();
        const a = e.currentTarget
        const href = a.getAttribute('href');
        $.ajax({
            url: href,
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function () {
                alert("Dodano do koszyka");
            },
            failure: function () {
                alert("Wystąpił błąd podczas dodawania do koszyka.");
            }
        });
    })
})

document.querySelectorAll('.deleteFromCart').forEach((elem) => {
    elem.addEventListener('click', async (e) => {
        e.preventDefault();
        const a = e.currentTarget
        const href = a.getAttribute('href');
        const price = parseInt(a.getAttribute('value'));
        var cartSummary = document.getElementById('cartSum');
        var cartSum = parseInt(cartSummary.getAttribute('value'));
        $.ajax({
            url: href,
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("X-XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function () {
                a.parentElement.parentElement.parentElement.remove();
                const newSum = cartSum - price;
                if (newSum > 0) {
                    cartSummary.setAttribute('value', newSum);
                    cartSummary.textContent = newSum;
                }
                else {
                    location.reload();
                }
            },
            failure: function () {
                alert("Wystąpił błąd podczas usuwania z koszyka.");
            }
        });
    })
})


var USDollar = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
});

document.addEventListener('input', () => {
    var quantityInput = document.getElementById("quantity");
    var basePrice = parseFloat(document.getElementById("details-form").getAttribute("data-price"));

    quantityInput.addEventListener("input", function () {
        var quantity = parseFloat(quantityInput.value);
        var price = calculatePrice(quantity, basePrice);

        document.getElementById("priceBase").textContent = USDollar.format(price.basePrice);
        document.getElementById("price50").textContent = USDollar.format(price.price50);
        document.getElementById("price100").textContent = USDollar.format(price.price100);
    });

    function calculatePrice(quantity, basePrice) {
        var discountRate50 = 0.9; // 10% discount
        var discountRate100 = 0.8; // 20% discount
        var finalPrice = basePrice * quantity;
        var price50 = quantity <= 50 ? finalPrice : basePrice * 50 * discountRate50 + basePrice * (quantity - 50);
        var price100 = quantity <= 100 ? finalPrice : basePrice * 100 * discountRate100 + basePrice * (quantity - 100);

        return {
            basePrice: finalPrice,
            price50: price50,
            price100: price100
        };
    }
});
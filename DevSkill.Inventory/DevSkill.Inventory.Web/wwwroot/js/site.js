//-- Item page content ---  
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchAllItems');
    const items = document.querySelectorAll('.item-box');
    const itemCountField = document.getElementById('itemCount');
    const totalQuantityField = document.getElementById('totalQuantity');
    const totalValueField = document.getElementById('totalValue');

    const checkboxes = document.querySelectorAll('.checkbox-delete');
    const selectedItemsDiv = document.getElementById('selectedItemsDiv');
    const selectedItemsText = document.getElementById('selectedItemsText');
    const selectAllItems = document.getElementById('selectAllItems');
    const clearSelection = document.getElementById('clearSelection');

    // Form submission when Enter key is pressed
    searchInput.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
        }
    });

    // Add event listeners to input search section. Update countable field.
    searchInput.addEventListener('input', function () {
        const searchTerm = searchInput.value.toLowerCase();
        let visibleItemCount = 0;
        let totalQuantity = 0;
        let totalValue = 0; 

        items.forEach(item => {
            const title = item.querySelector('.card-title').textContent.toLowerCase();
            if (title.includes(searchTerm)) {
                item.style.visibility = 'visible';
                item.style.opacity = '1';
                visibleItemCount++;

                const itemQuantity = parseInt(item.querySelector('.item-quantity').textContent.replace('Quantity: ', '')) || 0;
                const itemPrice = parseFloat(item.querySelector('.item-price').textContent.replace('Price: ', '').replace('$', '')) || 0;
                const itemTotalValue = itemQuantity * itemPrice; 

                totalQuantity += itemQuantity;
                totalValue += itemTotalValue;
            } else {
                item.style.visibility = 'hidden';
                item.style.opacity = '0';
            }
        });

        // Update countable fields
        itemCountField.textContent = visibleItemCount;
        totalQuantityField.textContent = totalQuantity;
        totalValueField.textContent = totalValue.toFixed(2) + ' $';
    });

    let selectedCount = 0;

    // Add event listeners to each checkbox
    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('click', function () {
            if (checkbox.checked) {
                checkboxes.forEach(function (cb) {
                    cb.style.display = 'block';
                });
            }
        });
    });

    // Function to update the selected items display
    function updateSelectedItemsDisplay() {
        selectedCount = Array.from(checkboxes).filter(cb => cb.checked).length;
        if (selectedCount > 0) {
            selectedItemsText.textContent = `${selectedCount} item${selectedCount > 1 ? 's' : ''} selected`;
            selectedItemsDiv.style.display = 'block';
        } else {
            selectedItemsText.textContent = `0 items selected`;
            selectedItemsDiv.style.display = 'none';
        }
    }

    // Add event listeners to each checkbox
    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('click', updateSelectedItemsDisplay);
    });

    // Event listener for "All" selection
    selectAllItems.addEventListener('click', function () {
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = true;
        });
        updateSelectedItemsDisplay();
    });

    // Event listener for "Clear Selection"
    clearSelection.addEventListener('click', function () {
        checkboxes.forEach(function (checkbox) {
            checkbox.checked = false;
        });
        updateSelectedItemsDisplay();
    });

});





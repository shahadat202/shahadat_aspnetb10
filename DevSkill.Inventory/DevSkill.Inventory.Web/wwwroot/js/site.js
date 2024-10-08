//-- Item page content ---
document.addEventListener('DOMContentLoaded', function () {
    var folderSearch = document.getElementById("folderSearch");
    var tagsContainer = document.getElementById("searchTagsContainer");
    var clearButton = document.getElementById("clear-button");

    var searchInput = document.getElementById('searchAllItems');
    var items = document.querySelectorAll('.item-box');
    var itemCountField = document.getElementById('itemCount');
    var totalQuantityField = document.getElementById('totalQuantity');
    var totalValueField = document.getElementById('totalValue');

    var checkboxes = document.querySelectorAll('.checkbox-delete');
    var selectedItemsDiv = document.getElementById('selectedItemsDiv');
    var selectedItemsText = document.getElementById('selectedItemsText');
    var selectAllItems = document.getElementById('selectAllItems');
    var clearSelection = document.getElementById('clearSelection');
    var deleteButton = document.querySelector('.show-bs-modal');

    // Form submission when Enter key is pressed
    searchInput.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
        }
    });

    // Input changes when search
    folderSearch.addEventListener("input", function () {
        var query = folderSearch.value.toLowerCase();
        var tags = tagsContainer.getElementsByClassName("tag-item");

        // Show or hide the clear button based on input value
        if (query.length > 0) {
            clearButton.style.display = "block";
        } else {
            clearButton.style.display = "none";
        }

        // Loop through each tag and filter based on the search query
        Array.from(tags).forEach(function (tag) {
            var tagText = tag.getElementsByClassName("tag-text")[0].textContent.toLowerCase();

            if (tagText.includes(query)) {
                tag.style.display = "block";
            } else {
                tag.style.display = "none";
            }
        });
    });

    // Show items serially when search
    searchInput.addEventListener('input', function () {
        var searchValue = searchInput.value.toLowerCase();
        var searchTerm = searchInput.value.toLowerCase();
        let visibleItemCount = 0;
        let totalQuantity = 0;
        let totalValue = 0;
        items.forEach(function (item) {
            var title = item.querySelector('.card-title').textContent.toLowerCase();

            if (title.includes(searchValue)) {
                item.style.display = 'flex';
                item.style.margin = '0px 45px 20px 40px';
                visibleItemCount++;

                var itemQuantity = parseInt(item.querySelector('.item-quantity').textContent.replace('Quantity: ', '')) || 0;
                var itemPrice = parseFloat(item.querySelector('.item-price').textContent.replace('Price: ', '').replace('$', '')) || 0;
                var itemTotalValue = itemQuantity * itemPrice;

                totalQuantity += itemQuantity;
                totalValue += itemTotalValue;
            } else {
                item.style.display = 'none';
            }
        });
        // After filtering, reflow the visible items
        var filteredItems = Array.from(items).filter(item => item.style.display === 'flex');

        var productsContainer = document.getElementById('products');
        productsContainer.innerHTML = '';
        filteredItems.forEach(item => productsContainer.appendChild(item));

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

    // Handle clear button click
    clearButton.addEventListener("click", function () {
        folderSearch.value = '';
        clearButton.style.display = "none";

        var tags = tagsContainer.getElementsByClassName("tag-item");
        Array.from(tags).forEach(function (tag) {
            tag.style.display = "block";
        });
    });

    // Event listener for delete button click
    deleteButton.addEventListener('click', function () {
        $('#modal-default').modal('show');
        var selectedIds = Array.from(checkboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value);

        document.getElementById('deleteId').value = selectedIds.join(',');
    });
});






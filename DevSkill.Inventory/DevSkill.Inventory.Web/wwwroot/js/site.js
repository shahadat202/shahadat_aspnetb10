document.addEventListener('DOMContentLoaded', function () {
    var folderSearch = document.getElementById("folderSearch");
    var tagsContainer = document.getElementById("searchTagsContainer");
    var titlesContainer = document.getElementById("searchTitlesContainer");
    var reportsContainer = document.getElementById("searchReportsContainer");
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

    // Input changes when search (mainly it's clickable function for title and tag search)
    function setupSearch(inputElement, containerElement, itemClass, textClass, clearButton) {
        inputElement.addEventListener("input", function () {
            var query = inputElement.value.toLowerCase();
            var items = containerElement.getElementsByClassName(itemClass);

            clearButton.style.display = query.length > 0 ? "block" : "none";

            Array.from(items).forEach(function (item) {
                var itemText = item.getElementsByClassName(textClass)[0].textContent.toLowerCase();
                item.style.display = itemText.includes(query) ? "block" : "none";
            });
        });
        clearButton.addEventListener("click", function () {
            inputElement.value = '';
            clearButton.style.display = "none";

            var items = containerElement.getElementsByClassName(itemClass);
            Array.from(items).forEach(function (item) {
                item.style.display = "block";
            });
        });
    }
    setupSearch(folderSearch, tagsContainer, "tag-item", "tag-text", clearButton);
    setupSearch(folderSearch, titlesContainer, "title-item", "title-text", clearButton);
    setupSearch(folderSearch, reportsContainer, "menu-item", "menu-text", clearButton);

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

    // Modal Delete Button
    deleteButton.addEventListener('click', function () {
        $('#modal-default').modal('show');
        var selectedIds = Array.from(checkboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value);

        document.getElementById('deleteId').value = selectedIds.join(',');
    });

    // Function to filter items based on specified criteria
    function filterItems(criteria, isTag) {
        let visibleItemCount = 0;
        let totalQuantity = 0;
        let totalValue = 0;

        items.forEach(function (item) {
            var itemContent = isTag
                ? item.querySelector('.item-tag').textContent.toLowerCase()
                : item.querySelector('.card-title').textContent.toLowerCase();

            if (itemContent.includes(criteria.toLowerCase())) {
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

        var filteredItems = Array.from(items).filter(item => item.style.display === 'flex');
        var productsContainer = document.getElementById('products');
        productsContainer.innerHTML = '';
        filteredItems.forEach(item => productsContainer.appendChild(item));

        itemCountField.textContent = visibleItemCount;
        totalQuantityField.textContent = totalQuantity;
        totalValueField.textContent = totalValue.toFixed(2) + ' $';
    }

    // Function to set up click event listeners for items
    function setupClickEvents(elements, activeClass, iconClass, filterFunction, isTag) {
        elements.forEach(function (element) {
            element.addEventListener('click', function () {
                var selectedText = this.querySelector('.' + iconClass).textContent;
                elements.forEach(function (el) {
                    el.classList.remove(activeClass);
                    el.querySelector('.' + iconClass).style.color = '';
                });

                this.classList.add(activeClass);
                this.querySelector('.' + iconClass).style.color = 'red';

                filterFunction(selectedText, isTag);
            });
        });
    }

    // Add click event listeners for tag elements
    var tagElements = document.querySelectorAll('.tag-item');
    setupClickEvents(tagElements, 'active-tag', 'tag-text', filterItems, true);

    // Add click event listeners for title elements
    var titleElements = document.querySelectorAll('.title-item');
    setupClickEvents(titleElements, 'active-title', 'title-text', filterItems, false);

});





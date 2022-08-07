
// Sortable 
export function assignMultipleSortableJS(containerClass, draggableClass, dotNetHelper)
{
    try
    {
        // https://sortablejs.github.io/Sortable/ 
        var elements = document.getElementsByClassName(containerClass);
        for (var i = 0; i < elements.length; i++) {
            var ele = elements[i];
            if (ele === null || ele === undefined)
                continue;

            assignSortableJS(ele, draggableClass, dotNetHelper)
        }
    }
    catch (e)
    {
        alert(e);
    }
}
function assignSortableJS(containerElement, dragabbleClass, dotNetHelper)
{
    try {

        // Init 
        var sortableDiv = new Sortable(containerElement, {
            group: 'shared',
            swapThreshold: 0.50,
            sort: true,  // sorting inside list
            delay: 0, // time in milliseconds to define when the sorting should start
            delayOnTouchOnly: false, // only delay if user is using touch
            touchStartThreshold: 0, // px, how many pixels the point should move before cancelling a delayed drag event
            disabled: false, // Disables the sortable if set to true.
            store: null,  // @see Store
            animation: 150,  // ms, animation speed moving items when sorting, `0` — without animation
            easing: "cubic-bezier(1, 0, 0, 1)", // Easing for animation. Defaults to null. See https://easings.net/ for examples.
            handle: ".grab",  // Drag handle selector within list items
            draggable: "." + dragabbleClass,  // Specifies which items inside the element should be draggable

            setData: function (dataTransfer, dragEl)
            {
                dataTransfer.setData('Text', dragEl.textContent); // `dataTransfer` object of HTML5 DragEvent
            },

            // Element is chosen
            onChoose: function (/**Event*/evt)
            {
                evt.oldIndex;  // element index within parent
            },

            // Element is unchosen
            onUnchoose: function (/**Event*/evt)
            {
                // same properties as onEnd
            },

            // Element dragging started
            onStart: function (/**Event*/evt)
            {
                evt.oldIndex;  // element index within parent
            },

            // Element dragging ended 
            onEnd: function (/**Event*/evt)
            {
                try
                {

                    var allitems = evt.to.children; // all items of parent 
                    for (var i = 0; i < allitems.length; i++) {
                        let item = allitems[i];
                        if (item === null)
                            continue;

                        let id = item.id;
                        let itemtype = getAttributeValue(item, "data-itemtype");
                        let ppid = getAttributeValue(item, "data-ppid");
                        let boardtype = getAttributeValue(item, "data-boardtype");
                        let sortorder = (i + 1).toString();
                        let columnitemtype = getAttributeValue(item, "data-columnitemtype");
                        let rowitemtype = getAttributeValue(item, "data-rowitemtype");
                        let filter = getAttributeValue(item, "data-filter");

                        // Create Parameter 
                        let jsonParameter = {
                            "GUID": id,
                            "ItemType": itemtype,
                            "PPID": ppid,
                            "BoardType": boardtype,
                            "SortOrder": sortorder,
                            "ColumnItemType": columnitemtype,
                            "RowItemType": rowitemtype,
                            "Filter": filter,
                        };

                        try {

                            dotNetHelper.invokeMethodAsync('OnDragEnd', jsonParameter).then(data => {

                                console.log(data);
                            });
                        }
                        catch (e) {
                            console.log("FAIL: " + e);
                        }

                    }

                } catch (e)
                {
                    alert(e);
                }
            },

            // Element is dropped into the list from another list
            onAdd: function (/**Event*/evt)
            {
                // ON Drop 
                try
                {
                    let item = evt.item;
                    if (item === null)
                        return;

                    let dropContainer = evt.to;
                    if (dropContainer === null)
                        return;

                    // let newIndex = evt.newIndex;

                    let col = getAttributeValue(dropContainer, "data-column");
                    let row = getAttributeValue(dropContainer, "data-row");

                    let id = item.id;
                    let itemtype = getAttributeValue(item, "data-itemtype");
                    let ppid = getAttributeValue(item, "data-ppid");
                    let boardtype = getAttributeValue(item, "data-boardtype");
                    let sortorder = "1";
                    let columnitemtype = getAttributeValue(item, "data-columnitemtype");
                    let rowitemtype = getAttributeValue(item, "data-rowitemtype");
                    let filter = getAttributeValue(item, "data-filter");

                    // Create Parameter 
                    let jsonParameter = {
                        "GUID": id,
                        "ItemType": itemtype,
                        "PPID": ppid,
                        "Column": col,
                        "Row": row,
                        "BoardType": boardtype,
                        "SortOrder": sortorder,
                        "ColumnItemType": columnitemtype,
                        "RowItemType": rowitemtype,
                        "Filter": filter,
                    };

                    try {

                        dotNetHelper.invokeMethodAsync('OnItemAdded', jsonParameter).then(data => {

                            console.log(data);
                        });
                    }
                    catch (e) {
                        console.log("FAIL: " + e);
                    }
                }
                catch (e) {
                    alert(e);
                }
            },

            // Changed sorting within list
            onUpdate: function (/**Event*/evt)
            {
                // same properties as onEnd 
                try
                {
                    let item = evt.item;
                    if (item === null)
                        return;

                    let dropContainer = evt.to;
                    if (dropContainer === null)
                        return;

                    let newIndex = evt.newIndex.toString();

                    let col = getAttributeValue(dropContainer, "data-column");
                    let row = getAttributeValue(dropContainer, "data-row");

                    let id = item.id;
                    let itemtype = getAttributeValue(item, "data-itemtype");
                    let ppid = getAttributeValue(item, "data-ppid");
                    let boardtype = getAttributeValue(item, "data-boardtype");
                    let sortorder = newIndex;
                    let columnitemtype = getAttributeValue(item, "data-columnitemtype");
                    let rowitemtype = getAttributeValue(item, "data-rowitemtype");
                    let filter = getAttributeValue(item, "data-filter");

                    // Create Parameter 
                    let jsonParameter = {
                        "GUID": id,
                        "ItemType": itemtype,
                        "PPID": ppid,
                        "Column": col,
                        "Row": row,
                        "BoardType": boardtype,
                        "SortOrder": sortorder,
                        "ColumnItemType": columnitemtype,
                        "RowItemType": rowitemtype,
                        "Filter" : filter,
                    };

                    try {

                        dotNetHelper.invokeMethodAsync('OnSortOrderChanged', jsonParameter).then(data => {

                            console.log(data);
                        });
                    }
                    catch (e) {
                        console.log("FAIL: " + e);
                    }

                }
                catch (e)
                {
                    alert(e);
                }
            },

            // Element is removed from the list into another list
            onRemove: function (/**Event*/evt) {
                // same properties as onEnd

                // same properties as onEnd 
                try {
                    let item = evt.item;
                    if (item === null)
                        return;

                    let dropContainer = evt.to;
                    if (dropContainer === null)
                        return;

                    let newIndex = evt.newIndex.toString();

                    let col = getAttributeValue(dropContainer, "data-column");
                    let row = getAttributeValue(dropContainer, "data-row");

                    let id = item.id;
                    let itemtype = getAttributeValue(item, "data-itemtype");
                    let ppid = getAttributeValue(item, "data-ppid");
                    let boardtype = getAttributeValue(item, "data-boardtype");
                    let sortorder = newIndex;
                    let columnitemtype = getAttributeValue(item, "data-columnitemtype");
                    let rowitemtype = getAttributeValue(item, "data-rowitemtype");
                    let filter = getAttributeValue(item, "data-filter");

                    // Create Parameter 
                    let jsonParameter = {
                        "GUID": id,
                        "ItemType": itemtype,
                        "PPID": ppid,
                        "Column": col,
                        "Row": row,
                        "BoardType": boardtype,
                        "SortOrder": sortorder,
                        "ColumnItemType": columnitemtype,
                        "RowItemType": rowitemtype,
                        "Filter": filter,
                    };

                    try {

                        dotNetHelper.invokeMethodAsync('OnItemRemoved', jsonParameter).then(data => {

                            console.log(data);
                        });
                    }
                    catch (e) {
                        console.log("FAIL: " + e);
                    }

                }
                catch (e) {
                    alert(e);
                }
            },

            // Called when dragging element changes position
            onChange: function (/**Event*/evt) {
                evt.newIndex // most likely why this event is used is to get the dragging element's current index
                // same properties as onEnd
            }
        });
    }
    catch (e)
    {
        alert("Fehler: " + e);
    }

}

// Attributes 
function assignAttribute(theNode, attributeName, attributeValue)
{
    try
    {
        var theAttribute = document.createAttribute(attributeName);
        theAttribute.value = attributeValue;
        theNode.setAttributeNode(theAttribute);
    } catch (e)
    {
        console.log("Creation of Attribute: " + e);
    }
}
function getAttributeValue(theNode, attributeName)
{
    try
    {
        var theValue = theNode.getAttribute(attributeName);
        if (theValue !== null)
        {
            return theValue;
        }
    }
    catch (e)
    {
        console.log("Getting an Attribute: " + e);
    }

    return null;
}

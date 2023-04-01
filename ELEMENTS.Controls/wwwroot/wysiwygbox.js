

export function loadEditor(editorID, modalID, saveBtnID, visualViewID, dotNetHelper)
{
    try
    {
        // define vars
        let editor = document.getElementById(editorID);
        let saveBTN = document.getElementById(saveBtnID);

        let toolbar = editor.getElementsByClassName('editor-toolbar')[0];
        let buttons = toolbar.querySelectorAll('.editor-btn:not(.has-submenu)');

        let contentArea = editor.getElementsByClassName('content-area')[0];
        let visuellView = document.getElementById(visualViewID);

        // add paste event
        visuellView.addEventListener('paste', pasteEvent);

        // add paragraph tag on new line
        contentArea.addEventListener('keypress', addParagraphTag);

        // SAVE
        saveBTN.addEventListener('click', function (e)
        {
            try
            {
                let localView = document.getElementById(visualViewID);

                var text = localView.innerHTML;

                // JSON Parameter 
                let json = {
                    "Text": localView.innerText,
                    "HTML": localView.innerHTML,
                };

                // call
                dotNetHelper.invokeMethodAsync('Save', json).then(data => {

                    console.log("saved");

                });
            }
            catch (e)
            {
                alert('fail:' + e);
                console.log("FAIL: " + e);
            }

        });

        // add toolbar button actions
        for (let i = 0; i < buttons.length; i++) {
            let button = buttons[i];

            button.addEventListener('click', function (e)
            {
                let action = this.dataset.action;

                switch (action)
                {
                    case 'insertTable':
                        {
                            insertTable(this, editor);
                            break;
                        }
                    default:
                        execDefaultAction(action);
                }

            });
        }

    }
    catch (e)
    {
        alert(e);
    }

}

function insertTable()
{
    try
    {
        let table = getHtmlTable();

        document.execCommand("insertHTML", false, table);
    }
    catch (e)
    {
        alert(e);
    }
}

function getHtmlTable()
{
    try {
        let table = '<table class="table">';
        table += '<thead><tr><th></th><th></th><th></th></tr></thead>';
        table += '<tbody>';
        table += '<tr><td></td><td></td><td></td></tr>';
        table += '<tr><td></td><td></td><td></td></tr>';
        table += '<tr><td></td><td></td><td></td></tr>';
        table += '</tbody>';
        table += '</table>';

        return table;
    }
    catch (e) {
        alert(e);
    }

    return '';
}

/**
 * This function adds a link to the current selection
 */
function execLinkAction() {
    modal.style.display = 'block';
    let selection = saveSelection();

    let submit = modal.querySelectorAll('button.done')[0];
    let close = modal.querySelectorAll('.close')[0];

    // done button active => add link
    submit.addEventListener('click', function (e) {
        e.preventDefault();
        let newTabCheckbox = modal.querySelectorAll('#new-tab')[0];
        let linkInput = modal.querySelectorAll('#linkValue')[0];
        let linkValue = linkInput.value;
        let newTab = newTabCheckbox.checked;

        restoreSelection(selection);

        if (window.getSelection().toString()) {
            let a = document.createElement('a');
            a.href = linkValue;
            if (newTab) a.target = '_blank';
            window.getSelection().getRangeAt(0).surroundContents(a);
        }

        modal.style.display = 'none';
        linkInput.value = '';

        // deregister modal events
        submit.removeEventListener('click', arguments.callee);
        close.removeEventListener('click', arguments.callee);
    });

    // close modal on X click
    close.addEventListener('click', function (e) {
        e.preventDefault();
        let linkInput = modal.querySelectorAll('#linkValue')[0];

        modal.style.display = 'none';
        linkInput.value = '';

        // deregister modal events
        submit.removeEventListener('click', arguments.callee);
        close.removeEventListener('click', arguments.callee);
    });
}


/**
 * This function executes all 'normal' actions
 */
function execDefaultAction(action) {
    document.execCommand(action, false);
}

/**
 * Saves the current selection
 */
function saveSelection()
{
    if (window.getSelection)
    {
        sel = window.getSelection();
        if (sel.getRangeAt && sel.rangeCount)
        {
            let ranges = [];
            for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                ranges.push(sel.getRangeAt(i));
            }
            return ranges;
        }
    }
    else if (document.selection && document.selection.createRange) {
        return document.selection.createRange();
    }
    return null;
}

/**
 *  Loads a saved selection
 */
function restoreSelection(savedSel) {
    if (savedSel) {
        if (window.getSelection) {
            sel = window.getSelection();
            sel.removeAllRanges();
            for (var i = 0, len = savedSel.length; i < len; ++i) {
                sel.addRange(savedSel[i]);
            }
        } else if (document.selection && savedSel.select) {
            savedSel.select();
        }
    }
}


/**
 * Checks if the passed child has the passed parent
 */
function childOf(child, parent) {
    return parent.contains(child);
}

/**
 * Sets the tag active that is responsible for the current element
 */
function parentTagActive(elem) {
    if (!elem || !elem.classList || elem.classList.contains('visuell-view')) return false;

    let toolbarButton;

    // active by tag names
    let tagName = elem.tagName.toLowerCase();
    toolbarButton = document.querySelectorAll(`.toolbar .editor-btn[data-tag-name="${tagName}"]`)[0];
    if (toolbarButton) {
        toolbarButton.classList.add('active');
    }

    // active by text-align
    let textAlign = elem.style.textAlign;
    toolbarButton = document.querySelectorAll(`.toolbar .editor-btn[data-style="textAlign:${textAlign}"]`)[0];
    if (toolbarButton) {
        toolbarButton.classList.add('active');
    }

    return parentTagActive(elem.parentNode);
}

/**
 * Handles the paste event and removes all HTML tags
 */
function pasteEvent(e) {
    e.preventDefault();

    let text = (e.originalEvent || e).clipboardData.getData('text/plain');
    document.execCommand('insertHTML', false, text);
}

/**
 * This functions adds a paragraph tag when the enter key is pressed
 */
function addParagraphTag(evt) {
    if (evt.keyCode == '13') {

        // don't add a p tag on list item
        if (window.getSelection().anchorNode.parentNode.tagName === 'LI') return;
        document.execCommand('formatBlock', false, 'p');
    }
}
window.helpdeskRichText = {
    format(editorId, command, value) {
        const editor = document.getElementById(editorId);
        if (!editor) return;
        editor.focus();
        document.execCommand(command, false, value || null);
        editor.dispatchEvent(new Event('input', { bubbles: true }));
    },
    addLink(editorId) {
        const editor = document.getElementById(editorId);
        if (!editor) return;
        const url = window.prompt('Enter the link address');
        if (!url) return;
        editor.focus();
        document.execCommand('createLink', false, url);
        editor.dispatchEvent(new Event('input', { bubbles: true }));
    },
    addComment(editorId, heading) {
        const editor = document.getElementById(editorId);
        if (!editor) return;
        editor.focus();
        if (editor.innerHTML.trim()) editor.appendChild(document.createElement('hr'));
        const commentHeading = document.createElement('p');
        const author = document.createElement('strong');
        author.textContent = heading;
        commentHeading.appendChild(author);
        const commentBody = document.createElement('p');
        const prompt = document.createTextNode('Comment: ');
        commentBody.appendChild(prompt);
        editor.appendChild(commentHeading);
        editor.appendChild(commentBody);
        const selection = window.getSelection();
        const range = document.createRange();
        range.setStart(commentBody, 1);
        range.collapse(true);
        selection.removeAllRanges();
        selection.addRange(range);
    },
    setValue(editorId, value) {
        const editor = document.getElementById(editorId);
        if (editor) editor.innerHTML = value || '';
    },
    getValue(editorId) {
        return document.getElementById(editorId)?.innerHTML ?? '';
    }
};

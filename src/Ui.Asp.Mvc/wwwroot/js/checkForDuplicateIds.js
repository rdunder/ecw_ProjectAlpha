//  ClaudeAi helped with refining these function.
//  The idea was to check if there are any duplicate id's in the html
//  And to get a simple to read overview so it can be fixed.

/**
 * Finds and reports all elements with duplicate IDs in the document
 * @returns {Object} Result object containing duplicate information
 */
function findDuplicateIds() {
    const elementsWithIds = document.querySelectorAll('[id]');
    const allIds = Array.from(elementsWithIds).map(el => el.id);

    const duplicateIds = allIds.filter((id, index) => allIds.indexOf(id) !== index);
    const uniqueDuplicateIds = [...new Set(duplicateIds)];

    const hasDuplicates = duplicateIds.length > 0;

    const duplicateDetails = [];

    if (hasDuplicates) {

        uniqueDuplicateIds.forEach(dupId => {
            const elementsWithSameId = document.querySelectorAll(`#${CSS.escape(dupId)}`);

            const instances = [];

            elementsWithSameId.forEach((el, i) => {
                const path = getPathToElement(el);

                instances.push({
                    element: el,
                    path: path
                });
            });

            duplicateDetails.push({
                id: dupId,
                count: elementsWithSameId.length,
                elements: instances
            });
        });
    }

    return {
        hasDuplicates,
        duplicateIds: uniqueDuplicateIds,
        details: duplicateDetails
    };
}

/**
 * Helper function to get a simple path to the element
 * @param {HTMLElement} element - The element to get the path for
 * @returns {string} A CSS-like path to the element
 */
function getPathToElement(element) {
    let path = '';
    let current = element;

    while (current && current !== document.body) {
        let name = current.tagName.toLowerCase();
        if (current.className && typeof current.className === 'string') {
            name += `.${current.className.trim().replace(/\s+/g, '.')}`;
        }
        if (current.id) {
            name += `#${current.id}`;
        }
        path = name + (path ? ' > ' + path : '');
        current = current.parentElement;
    }

    return path ? 'body > ' + path : path;
}



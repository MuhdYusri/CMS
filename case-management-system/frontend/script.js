document.addEventListener("DOMContentLoaded", function () {
    const caseForm = document.getElementById("caseForm");
    const caseList = document.getElementById("caseList");
    const searchName = document.getElementById("searchName");
    const filterChannel = document.getElementById("filterChannel");

    let cases = [];

    // Add new case
    caseForm.addEventListener("submit", function (e) {
        e.preventDefault();
        const customerName = document.getElementById("customerName").value;
        const caseChannel = document.getElementById("caseChannel").value;

        if (customerName) {
            cases.push({ name: customerName, channel: caseChannel });
            document.getElementById("customerName").value = ""; // Clear input
            renderCases();
        }
    });

    // Render cases in table
    function renderCases() {
        caseList.innerHTML = "";
        let filteredCases = cases.filter(c =>
            c.name.toLowerCase().includes(searchName.value.toLowerCase()) &&
            (filterChannel.value === "" || c.channel === filterChannel.value)
        );

        filteredCases.forEach((c, index) => {
            let row = document.createElement("tr");
            row.innerHTML = `
                <td>${c.name}</td>
                <td>${c.channel}</td>
                <td>
                    <button onclick="deleteCase(${index})">Delete</button>
                </td>
            `;
            caseList.appendChild(row);
        });
    }

    // Delete case
    window.deleteCase = function (index) {
        cases.splice(index, 1);
        renderCases();
    };

    // Search & Filter event listeners
    searchName.addEventListener("input", renderCases);
    filterChannel.addEventListener("change", renderCases);
});

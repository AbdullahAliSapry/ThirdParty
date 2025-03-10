const urlParams = new URLSearchParams(window.location.search);

const userId = urlParams.get('userid');




document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("commissionForm");

    if (!form) {
        console.error("⚠️ commissionForm not found in the DOM.");
        toastr.error("Form not found. Please refresh the page.");
        return;
    }

    form.addEventListener("submit", async (event) => {
        event.preventDefault();
        console.log("📤 Form submission initiated.");

        // Extract and validate form inputs
        const revokedAtInput = document.getElementById("revokedAt");
        const discountRateInput = document.getElementById("discountRate");
        const isActiveInput = document.getElementById("isActive");
        const userIdInput = document.getElementById("userId");
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');

        if (!revokedAtInput || !discountRateInput || !isActiveInput || !userIdInput || !tokenInput) {
            console.error("❌ One or more form elements are missing.");
            toastr.error("Form elements missing. Please refresh the page.");
            return;
        }

        const revokedAt = revokedAtInput.value;
        const discountRate = discountRateInput.value ? parseFloat(discountRateInput.value) : null;
        const isActive = isActiveInput.checked;
        const userId = userIdInput.value;
        const requestToken = tokenInput.value;

        if (!revokedAt) {
            toastr.error("⚠️ Please select a revoked date.");
            return;
        }

        const requestData = { IsActive: isActive, RevokedAt: revokedAt, discountRate };

        try {
            const response = await fetch(`/Marketer/AddCodeToMarketer/${userId}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": requestToken,
                },
                body: JSON.stringify(requestData),
            });

            let dataResponse = null;
            const contentType = response.headers.get("content-type");

            if (contentType?.includes("application/json")) {
                dataResponse = await response.json();
            } else {
                console.warn("⚠️ Response is not JSON.");
                toastr.error("Invalid server response format.");
                return;
            }

            if (response.ok) {
                toastr.success(dataResponse?.message || "✅ Added successfully!");
                updateTableWithResponseData(dataResponse);
                form.reset();
                closeModal();
            } else {
                console.error("🚫 Server responded with error:", dataResponse);
                toastr.error(dataResponse?.message || "❌ An error occurred while adding.");
            }

        } catch (err) {
            console.error("🚨 Network or server error:", err);
            toastr.error(err?.message || "⚠️ Unexpected error occurred.");
        }
    });

    function updateTableWithResponseData(data) {
        const tableBody = document.querySelector(".table-container > table");

        if (!tableBody) {
            console.error("⚠️ Table body not found.");
            toastr.error("Unable to update table.");
            return;
        }

        if (!data || typeof data !== "object") {
            console.error("⚠️ Invalid response data:", data);
            return;
        }


        const safeValue = (value, fallback = "No data") => value ?? fallback;

        const htmlRow = `<tr>
                    <td>${safeValue(data.code, "N/A")}</td>
                    <td>0</td>
                    <td>${new Date(safeValue(data.createAt)).toDateString()}</td>
                    <td>${new Date(safeValue(data.revokedAt)).toDateString()}</td>
                    <td>${data.isActive ? "Yes" : "No"}</td>
                    <td>${data.discountRate ?? "error in data"}</td>
                    <td>
                        <a href="/Marketer/GetAllDetailsToCode?userId=${userId}&code=${data.code}"
                           class="btn details-btn">Show Details</a>
                        <a href="/Marketer/ChangeStatusToCode?userId=${userId}&code=${data.code}"
                           class="btn ${data.isActive ? 'revoke-btn' : ''}">
                            ${data.isActive ? "Revoke" : "Active"}
                        </a>
                    </td>
                    <td class="delete-btn">
                        <a href="/Marketer/DeleteCode?codeId=${data.id}&code=${data.code}&userId=${userId}">
                            <i class="fa-solid fa-trash"></i>
                        </a>
                    </td>
                </tr>
                `;

        try {
            tableBody.insertAdjacentHTML("beforeend", htmlRow);
        } catch (err) {
            console.error("🚫 Error adding row to table:", err);
            toastr.error("Failed to add row. Please try again.");
        }
    }
});

// Open Modal
document.getElementById("openModal")?.addEventListener("click", function () {
    document.getElementById("commissionModal").classList.add("active");
    document.querySelector(".modal-overlay").style.display = "block";
});

function closeModal() {
    document.getElementById("commissionModal").classList.remove("active");
    document.querySelector(".modal-overlay").style.display = "none";
}


//document.querySelector(".btn-save-com").addEventListener("click", saveCommission);

// Save Commission (Dummy Function)
//function saveCommission() {
//    alert("Commission saved successfully!");
//    closeModal();
//}


document.querySelector(".details-btn")?.addEventListener("click", showDetails);


// Show Details Page
function showDetails() {
    document.getElementById("detailsPage").style.display = "block";
}
function callFreelancer() {
    const username = usernameFromSession;
    const currentrole = currentroleFromSession;

    const account = {
        UserName: username,
        currentrole: currentrole
    };

    const link = "https://localhost:7265/api/Account/update-role";

    $.ajax({
        url: link,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(account),
        success: function (apiResponse) {
            if (apiResponse.isSuccess) {
                const token = apiResponse.data.toString();
                const handler = new JwtSecurityTokenHandler();
                const jsonToken = handler.ReadToken(token);

                const userId = jsonToken.Claims.find(claim => claim.Type === "UserId").Value;
                const currentRole = jsonToken.Claims.find(claim => claim.Type === "role").Value;

                sessionStorage.setItem("UserId", userId);
                sessionStorage.setItem("token", token);
                sessionStorage.setItem("currentRole", currentRole);

                window.location.href = "/Index?token=" + token;
            } else {
                if (apiResponse.code === 0) {
                    console.error("Login error");
                } else {
                    console.error(apiResponse.message);
                }
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
        }
    });
}

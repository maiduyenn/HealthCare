$(document).ready(function () {
    var _API_URL = "https://localhost:7102/";
    var mealId = 1;
    var containerInformation = "breakfast";
    var meals = [
        {
            type: "breakfast",
            id: 1,
            foods: []
        }, {
            type: "lunch",
            id: 2,
            foods: []
        }, {
            type: "dinner",
            id: 3,
            foods: []
        },
        {
            type: "snacks",
            id: 4,
            foods: []
        }
    ];
    var waterVolume = 0;
    var activities = [];


    function submit() {
        const request = {
            meal: meals,
            water: waterVolume,
            activities: activities
        };
        $.ajax({
            type: "POST",
            url: "/Home/Save",
            data: JSON.stringify(request),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                    window.location.href = '/Home/AthleteInformation';
            },
            error: function () {
                alert("Something went wrong!");
            }
        });  
    }


    $("#savechanges").on("click", function () {
        submit();
    })


    $(".food-container .form-check input").on("change", function () {
        if ($(this).is(":checked")) {
            containerInformation = this.id;
            mealId = +$(this).attr("data-id");
        }
    })

    $("#buttonAddWater").on("click", function () {
        waterVolume += 250;
        $("#waterVolume").text(waterVolume + " ml");
        $("#waterVolume").css("color", "black");

    })

    $("#addButton").on("click", function () {
        const foodName = $("#foodSelector").find(":selected").text();
        const foodValue = $("#foodSelector").val();
        let meal = meals.find(x => x.id === mealId);
        meal.foods.push(foodValue);
        console.log(meal);
        $(`#container-${containerInformation}`).append(`<div style="color:blue;">${foodName}</div>`);
    })

    $("#addExercises").on("click", function () {
        const exerciseName = $("#exerciseSelector").find(":selected").text();
        const exerciseValue = $("#exerciseSelector").val();
        $(`#container-activities`).append(`<div style="color:blue;">${exerciseName}</div>`);
        activities.push(exerciseValue);
    })


})


window.createCharts = function (
    open,
    closed,
    high,
    medium,
    low) {

    console.log("createCharts called");
    console.log(typeof Chart);

    console.log(open, closed, high, medium, low);

    const statusCanvas =
        document.getElementById('statusChart');

    if (statusCanvas) {
        new Chart(statusCanvas, {
            type: 'pie',
            data: {
                labels: ['Open', 'Closed'],
                datasets: [{
                    data: [open, closed],
                    backgroundColor: [
                        '#198754',
                        '#6c757d'
                    ]
                }]
            }
        });
    }

    const priorityCanvas =
        document.getElementById('priorityChart');

    if (priorityCanvas) {
        new Chart(priorityCanvas, {
            type: 'bar',
            data: {
                labels: ['High', 'Medium', 'Low'],
                datasets: [{
                    label: 'Tickets',
                    data: [high, medium, low],
                    backgroundColor: [
                        '#dc3545',
                        '#ffc107',
                        '#198754'
                    ]
                }]
            }
        });
    }
}
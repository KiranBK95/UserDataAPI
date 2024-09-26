This is a simple ASP.NET Core Web API for handling student data submission as requested in the task, which allows users to submit their name, email, and academic year. 
ASP.NET Core Web API:
Built using ASP.NET Core 8 for handling API requests.
Entity Framework Core with SQL Server is used for data storage and management that are submitted from the react form via api.
The API is secured using JWT  authentication to authorize requests, since there was no instructions given for login/ etc.., just though of making it ,and wanted to give a try

    React.js Frontend:
        A simple form allows users to input their name, email, and academic year.
        Axios is used for sending HTTP requests from React to the backend API.
        After submission, the form sends a POST request to the API along with a JWT token to ensure that only authorized users can submit data.

    Authentication:
        The backend API issues a JWT token for form submissions.
        The AuthController provides a /api/auth/token endpoint to retrieve a token for the user when the form is loaded.
        The token is used in the Authorization header of subsequent API requests for secure form submission.

    Authorization:
        JWT tokens are validated server-side, checking for validity and role-based access control.
        Only requests with valid tokens containing the "Guest" role can access the form submission endpoint.

    Swagger UI:
        it provides swagger ui testing as well.

API Endpoints:

    POST /api/UserDetails:
        Secured endpoint for submitting student data.
        Requires a valid JWT token in the Authorization header (Bearer <token>).
    GET /api/auth/token:
        Public endpoint to retrieve a JWT token. This token is required to submit student data.

How It Works:

    React Frontend:
        The frontend displays a simple registration form where users input their name, email, and academic year.
        When the form is submitted, a token is fetched from the API, and the registration data is sent to the backend using Axios with the token in the request headers.
    ASP.NET Core API:
        Upon receiving the data, the API validates the token to ensure the request is authorized.
        If valid, the API stores the student data in the database using Entity Framework.
        The API responds with a success message or an error message, which is then displayed in the frontend.

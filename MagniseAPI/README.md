Overview
This documentation provides information about the Market Asset Price Information API Service. The API service allows users to retrieve price information for specific market assets.

Endpoints
The API service exposes the following endpoints:

Get List of Supported Market Assets

URL: /api/assets
Method: GET
Description: Retrieves a list of supported market assets.
Response: JSON array containing details of supported market assets.

Get Price Information for Specific Asset(s)

URL: /api/pricecollections/{assetIds}
Method: GET
Description: Retrieves price information for a specific asset identified by {assetId}. This endpoint also includes the time of the last update for each asset.
Parameters:
{assetId}: Identifier of the asset for which price information is requested.
Response: JSON array containing price information for the specified asset(s), including timestamp of the update.

Dockerfile: Used to build Docker image for deployment.

Running the Application
To run the Market Asset Price Information API Service, follow these steps:

Clone the Repository: Clone the repository containing the API service code.

Configuration:

Update appsettings.json with appropriate values for FintatechApi credentials and database connection string under ConnectionStrings.

Build the Docker Image:

Navigate to the root directory of the project where  is MagniseAPI.sln located.
Run the following command to build the Docker image:

docker build . -f MagniseAPI/Dockerfile -t magniseapi:2.0.0

Run the Docker Container:

Once the image is built, run the Docker container using:

docker run -p 8080:8080 magniseapi:2.0.0

Accessing the API:

The API service will be accessible at http://localhost:8080.
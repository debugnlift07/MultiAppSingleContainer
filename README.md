# MultiApp Single Container BFF (POC)

<p>
A lightweight microservices proof of concept built using <b>ASP.NET Core (.NET 8), Docker, and Swagger</b>. The solution demonstrates how multiple APIs can run inside a single Docker container while exposing only a Backend For Frontend (BFF) endpoint to consumers.
</p>

<div class="section box">
<h2>Architecture Overview</h2>

<pre>
Client Request
      ↓
App1 (BFF)
Port 8080
      ↓
 ┌───────────────┬───────────────┐
 │                               │
 ▼                               ▼
App2                       App3
Temperature Service        City Service
Port 8081                  Port 8082
</pre>

<h3>Components</h3>
<ul>
<li><b>App1 (BFF):</b> Aggregates responses from internal services and exposes a single endpoint.</li>
<li><b>App2:</b> Returns temperature information.</li>
<li><b>App3:</b> Returns city information.</li>
<li><b>Docker:</b> Hosts all three applications inside a single container.</li>
<li><b>Swagger:</b> Provides API documentation and testing interface.</li>
</ul>
</div>

<div class="section box">
<h2>Application Endpoints</h2>

<h3>App2 - Temperature Service</h3>

<pre>
GET /api/Temp

Response:
48C
</pre>

<h3>App3 - City Service</h3>

<pre>
GET /api/CityName

Response:
Noida
</pre>

<h3>App1 - Aggregated Endpoint</h3>

<pre>
GET /api/CityDetails/GetCityDetails

Response:
{
  "city": "Noida",
  "temperature": "48C"
}
</pre>

</div>

<div class="section box">
<h2>Internal Service Communication</h2>

<p>
App1 communicates with App2 and App3 internally using localhost URLs within the same Docker container.
</p>

<pre>
var cityTask =
    _httpClient.GetStringAsync(
        "http://localhost:8082/api/CityName");

var tempTask =
    _httpClient.GetStringAsync(
        "http://localhost:8081/api/Temp");
</pre>

</div>

<div class="section box">
<h2>Container Architecture</h2>

<pre>
Docker Container
│
├── App1 (Port 8080)
├── App2 (Port 8081)
└── App3 (Port 8082)

Only Port 8080 is exposed externally.
Ports 8081 and 8082 remain internal.
</pre>

</div>

<div class="section box">
<h2>Project Structure</h2>

<pre>
MultiAppSingleContainer
│
├── App1
│   ├── Controllers
│   └── App1.csproj
│
├── App2
│   ├── Controllers
│   └── App2.csproj
│
├── App3
│   ├── Controllers
│   └── App3.csproj
│
└── Dockerfile
</pre>

</div>

<div class="section box">
<h2>How to Run Locally</h2>

<h3>1. Build Docker Image</h3>

<pre>
docker build -t multiapp:v1 .
</pre>

<h3>2. Run Container</h3>

<pre>
docker run -d --name multiapp -p 8080:8080 multiapp:v1
</pre>

<h3>3. Verify Container</h3>

<pre>
docker ps
</pre>

<h3>4. View Logs</h3>

<pre>
docker logs multiapp
</pre>

</div>

<div class="section box">
<h2>Dockerfile</h2>

<pre>
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

RUN dotnet publish App1/App1.csproj -c Release -o /publish/App1
RUN dotnet publish App2/App2.csproj -c Release -o /publish/App2
RUN dotnet publish App3/App3.csproj -c Release -o /publish/App3

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /publish/App1 ./App1
COPY --from=build /publish/App2 ./App2
COPY --from=build /publish/App3 ./App3

EXPOSE 8080

CMD sh -c "dotnet /app/App2/App2.dll --urls=http://0.0.0.0:8081 & \
           dotnet /app/App3/App3.dll --urls=http://0.0.0.0:8082 & \
           dotnet /app/App1/App1.dll --urls=http://0.0.0.0:8080"
</pre>

</div>

<div class="section box">
<h2>Testing</h2>

<h3>Swagger UI</h3>

<pre>
http://localhost:8080/swagger/index.html
</pre>

<h3>Aggregated Endpoint</h3>

<pre>
http://localhost:8080/api/CityDetails/GetCityDetails
</pre>

Expected Response:

<pre>
{
  "city": "Noida",
  "temperature": "48C"
}
</pre>

</div>

<div class="section box">
<h2>Azure App Service Deployment</h2>

<h3>Build Image</h3>

<pre>
docker build -t multiapp:v1 .
</pre>

<h3>Tag Image</h3>

<pre>
docker tag multiapp:v1 &lt;acr-name&gt;.azurecr.io/multiapp:v1
</pre>

<h3>Push Image</h3>

<pre>
docker push &lt;acr-name&gt;.azurecr.io/multiapp:v1
</pre>

<h3>Deploy</h3>

<ul>
<li>Create Azure Container Registry (ACR)</li>
<li>Push image to ACR</li>
<li>Create Azure Linux App Service</li>
<li>Configure App Service to use container image</li>
<li>Expose Port 8080</li>
</ul>

</div>

<div class="section box">
<h2>Benefits of This POC</h2>

<ul>
<li>Single Docker Container Deployment</li>
<li>Backend For Frontend (BFF) Pattern</li>
<li>Internal Service-to-Service Communication</li>
<li>Reduced Infrastructure Complexity</li>
<li>Simple Azure App Service Hosting</li>
<li>Swagger Enabled APIs</li>
<li>Microservice Development Experience</li>
</ul>

</div>

<div class="section box">
<h2>Technologies Used</h2>

<ul>
<li>.NET 8 Web API</li>
<li>ASP.NET Core</li>
<li>Docker</li>
<li>Swagger / OpenAPI</li>
<li>HttpClient</li>
<li>REST APIs</li>
<li>Azure App Service</li>
</ul>

</div>

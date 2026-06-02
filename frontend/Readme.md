# Frontend - Logistics Delay Prediction System

## Overview
This is the Angular frontend for the AI Logistics Delay Prediction System. It provides a modern, responsive UI for managing shipments and predicting delivery delays using machine learning.

## Features

- **Dashboard**: View real-time statistics and metrics about shipments
- **Shipment Management**: Browse, search, and filter all shipments
- **Delay Prediction**: Use AI to predict potential delays for new shipments
- **Responsive Design**: Works seamlessly on desktop, tablet, and mobile devices

## Installation

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update the API URL in `src/environments/environment.ts` if needed:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7162/api' 
   };
   ```

## Development Server

Run the development server:
```bash
npm start
```

Navigate to `http://localhost:4200/`. 

## Building 

Build the project 
```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── dashboard/          # Main dashboard component
│   │   ├── shipment-list/      # Shipments list and search
│   │   └── prediction/         # AI delay prediction form
│   ├── services/
│   │   └── logistics.service.ts   # API communication service
│   ├── models/
│   │   └── logistics.model.ts     # Data models and interfaces
│   ├── app.component.*         # Root component
│   └── app.routes.ts           # Route definitions
├── environments/               # Environment configurations
├── assets/                     # Static assets
├── styles.scss                 # Global styles
├── main.ts                     # Application entry point
└── index.html                  # HTML template
```

## Components

### Dashboard Component
Displays key metrics:
- Total shipments count
- In-transit shipments
- Delivered shipments
- Delayed shipments
- Recent shipments table

### Shipment List Component
Features:
- View all shipments in a table
- Search by shipment number, origin, or destination
- Filter by status (pending, in-transit, delivered, delayed)
- Delete shipments

### Prediction Component
- Input shipment details (origin, destination, departure date, weight, distance)
- Get AI-powered delay predictions
- View risk levels and recommendations


## API Integration

The frontend communicates with the backend ASP.NET Core API. Ensure the API is running on:
- Development: `https://localhost:7162`

All API calls are handled through the `LogisticsService` in `src/app/services/logistics.service.ts`.


## Getting Started

To get started with the Angular frontend:

1. Install dependencies: `npm install`
2. Start the dev server: `npm start`
3. Open browser to `http://localhost:4200`


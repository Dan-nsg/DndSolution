# dnd-solution-frontend

This project is a Vue 3 frontend application built with Vite, designed to provide an interface for exploring and managing a spell grimoire.

## Features

- 🌟 **Spell Exploration**: Browse a collection of spells with filters and pagination.
- 🔍 **Search and Filters**: Search spells by name, school, or level.
- 📜 **Spell Details**: View detailed information about each spell, including components, duration, and description.
- 🔄 **Data Synchronization**: Sync spells with the backend API.
- 🌗 **Dark/Light Mode**: Toggle between light and dark themes.

## Screenshots

### Home Page
![Home Page](./screenshots/home.png)

### Spell List
![Spell List](./screenshots/spells-list.png)

### Spell Details
![Spell Details](./screenshots/spell-details.png)

> **Note**: Make sure to create a `screenshots` folder in the root directory and add the corresponding images.

## Technologies Used

- **Vue 3**: JavaScript framework for building reactive interfaces.
- **Pinia**: State management library.
- **Vue Router**: Routing for navigation between pages.
- **Axios**: HTTP client for API communication.
- **Ag-Grid**: Advanced table for data display.
- **FontAwesome**: Icons to enhance the UI.
- **Vite**: Build tool for fast development.

## Project Setup

1. Clone the repository:
   ```sh
   git clone https://github.com/your-username/dnd-solution-frontend.git
   cd dnd-solution-frontend

2. Install dependencies:
   npm install

3. Configure the .env file (if needed) to point to the backend API.

4. Start the development server:
   npm run dev

5. Access the application at http://localhost:5173.

## Available Scripts
npm run dev: Starts the development server.
npm run build: Builds the project for production.
npm run preview: Previews the production build.
npm run type-check: Runs TypeScript type checks.

## Project Structure
 src/
├── api/               # HTTP client configuration
├── assets/            # Static assets (CSS, images)
├── components/        # Reusable components
├── router/            # Route configuration
├── stores/            # State management with Pinia
├── views/             # Main pages
└── main.ts            # Application entry point

## Contribution
Contributions are welcome! Follow these steps to contribute:

1. Fork the repository.

2. Create a branch for your feature or fix:
    git checkout -b my-feature

3. Commit your changes:
    git commit -m "feat: add my new feature"

4. Push to the remote branch:
    git push origin my-feature

## License
This project is licensed under the MIT License.

Developed by Daniel Giani.

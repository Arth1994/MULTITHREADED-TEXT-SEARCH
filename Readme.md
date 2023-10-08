
# Data Entry and Search Application README

## Overview

This application is designed for data entry and multi-threaded text search functionality. It allows users to validate and store data about product rebates to a text file. The entered records are displayed in a list-view object within the application's user interface. Additionally, the application can perform multi-threaded text searches on a text file containing 18,000 sample records.

## Features

### Data Entry

- The application provides a user-friendly interface for entering data about product rebates.
- Validation checks ensure that the data entered is accurate and complete.
- Entered records are stored in a text file for future reference.

### List-View Display

- All entered records are displayed in a list-view object within the application.
- Users can easily view and manage the list of rebate records.

### Multi-Threaded Text Search

- The application includes a powerful text search feature.
- A background worker thread reads lines from a text file.
- A thread-safe queue object stores the lines read by the background worker thread.
- The main UI thread displays the search results in real-time as they become available.
- The multi-threaded search allows for efficient and responsive text searching, even in large datasets like the provided 18,000 sample records.

## Getting Started

Follow these steps to get started with the application:

1. **Installation**: Clone or download the application's source code.

2. **Dependencies**: Ensure that you have the required dependencies installed (e.g., .NET Framework, libraries).

3. **Run the Application**: Execute the application's main executable to launch the user interface.

4. **Data Entry**: Use the provided interface to enter and validate product rebate data.

5. **Text Search**: Utilize the multi-threaded text search feature to search the text file containing 18,000 sample records.

## Usage

- Data entry is intuitive and follows on-screen prompts.
- To perform a text search, input your search query and initiate the search.
- Search results will be displayed in real-time as they are found.

## Contributing

If you would like to contribute to the project, feel free to submit pull requests or reach out to the project maintainers.

## License

This project is open-source and licensed under [License Name]. See the [LICENSE](LICENSE) file for details.

## Contact

For questions or issues related to the application, please contact [Your Contact Information].

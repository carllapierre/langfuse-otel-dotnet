# Contributing to langfuse-otel-dotnet

Thank you for your interest in contributing! 🎉

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Git
- A code editor (Visual Studio, VS Code, or Rider)

### Setup Development Environment

1. **Fork and clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/langfuse-otel-dotnet.git
   cd langfuse-otel-dotnet
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the sample**
   ```bash
   # Set environment variables
   $env:OPENAI_API_KEY="sk-..."
   $env:LANGFUSE_PUBLIC_KEY="pk-lf-..."
   $env:LANGFUSE_SECRET_KEY="sk-lf-..."
   $env:LANGFUSE_BASE_URL="https://cloud.langfuse.com"
   
   # Run sample
   cd samples/SemanticKernel.Sample
   dotnet run
   ```

## How to Contribute

### Reporting Bugs
- Use the GitHub issue tracker
- Include a clear title and description
- Provide steps to reproduce
- Include code samples if possible
- Specify your .NET version and OS

### Suggesting Features
- Open a GitHub issue with the `enhancement` label
- Describe the use case and expected behavior
- Explain why this would be useful

### Submitting Pull Requests

1. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make your changes**
   - Write clean, readable code
   - Follow existing code style
   - Add tests if applicable
   - Update documentation if needed

3. **Commit your changes**
   ```bash
   git commit -m "Add: brief description of your change"
   ```
   
   Commit message format:
   - `Add:` for new features
   - `Fix:` for bug fixes
   - `Update:` for changes to existing features
   - `Docs:` for documentation changes

4. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```

5. **Open a Pull Request**
   - Go to the original repository on GitHub
   - Click "New Pull Request"
   - Select your branch
   - Fill out the PR template
   - Wait for review

## Code Style

- Follow standard C# conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise

## Testing

We currently don't have a test suite, but we're working on it! If you'd like to contribute tests, that would be amazing.

## Questions?

Feel free to open an issue with the `question` label or start a discussion!

## Code of Conduct

Please note that this project is released with a [Contributor Code of Conduct](CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.


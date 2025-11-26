# Testing Guide

This guide covers how to run tests for the Langfuse .NET SDK.

## Prerequisites

- .NET 9.0 SDK
- Docker (for local integration tests)

## Environment Setup

### 1. Create Environment File

Copy the example environment file and fill in your credentials:

```bash
cp .env.example .env
```

Edit `.env` with your Langfuse credentials:

## Running Tests

### Unit Tests Only

Run all unit tests (no external dependencies required):

```bash
dotnet test --filter "Category!=Integration"
```

### All Tests Including Integration

```bash
dotnet test
```

### Specific Test Projects

```bash
# Core library tests
dotnet test tests/Langfuse.Core.Tests

# Client library tests
dotnet test tests/Langfuse.Client.Tests

# Integration tests only
dotnet test tests/Langfuse.IntegrationTests
```

### Integration Tests

```bash
# Run integration tests (uses Cloud if configured, otherwise Local)
dotnet test --filter "Category=Integration"
```

## Integration Test Setup

The integration tests automatically detect which environment to use:
1. **Cloud** - if `LANGFUSE_PUBLIC_KEY` and `LANGFUSE_SECRET_KEY` are set
2. **Local** - if `LANGFUSE_LOCAL_PUBLIC_KEY` and `LANGFUSE_LOCAL_SECRET_KEY` are set
3. **Skip** - if neither is configured

### Cloud Tests

1. Create a Langfuse account at [cloud.langfuse.com](https://cloud.langfuse.com)
2. Create a project and get API keys from Project Settings
3. Create a test prompt named "test-prompt" (or set `LANGFUSE_TEST_PROMPT_NAME`)
4. Set environment variables as shown above

### Local Tests (Docker)

1. Start the local Langfuse instance:

```bash
cd docker
docker compose up -d

# Wait for services to be ready (2-3 minutes)
docker compose logs -f langfuse-web
# Wait until you see "Ready"
```

2. Access Langfuse at http://localhost:3003
3. Create an account (local auth, no email verification)
4. Create an organization and project
5. Get API keys from Project Settings
6. Update `.env` with local credentials

### Stopping Local Instance

```bash
cd docker
docker compose down

# To also remove data volumes
docker compose down -v
```

## Test Categories

| Category | Description | Requirements |
|----------|-------------|--------------|
| Unit | Tests without external dependencies | None |
| Integration | Tests against real Langfuse instance | Cloud OR Local API keys |

## Troubleshooting

### Tests are skipped

Integration tests skip if no environment is configured. Check:

```bash
# Cloud config
echo $LANGFUSE_PUBLIC_KEY
echo $LANGFUSE_SECRET_KEY

# Local config
echo $LANGFUSE_LOCAL_PUBLIC_KEY
echo $LANGFUSE_LOCAL_SECRET_KEY
```

### Local Docker not responding

```bash
# Check service status
cd docker
docker compose ps

# View logs
docker compose logs langfuse-web

# Restart services
docker compose restart
```

### Authentication errors

Verify your API keys are correct and have the necessary permissions in the Langfuse project settings.

## Writing New Tests

### Unit Tests

Place in `tests/Langfuse.*.Tests/`:

```csharp
public class MyFeatureTests
{
    [Fact]
    public void MyMethod_WhenCondition_ShouldResult()
    {
        // Arrange, Act, Assert
    }
}
```

### Integration Tests

Place in `tests/Langfuse.IntegrationTests/`. Tests auto-detect Cloud vs Local:

```csharp
[Trait("Category", "Integration")]
public class MyIntegrationTests
{
    [SkippableFact]
    public async Task MyMethod_IntegrationTest()
    {
        Skip.If(!TestConfiguration.IsCloudConfigured() && !TestConfiguration.IsLocalConfigured(), 
            "Requires Cloud or Local credentials");
        
        // Test implementation
    }
}
```


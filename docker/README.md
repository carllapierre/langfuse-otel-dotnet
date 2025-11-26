# Langfuse Local Development Environment

This Docker Compose setup provides a local Langfuse instance for development and testing.

## Quick Start

```bash
# Start Langfuse
docker compose up -d

# Wait for services to be ready (about 2-3 minutes)
docker compose logs -f langfuse-web
# Wait until you see "Ready"

# Stop Langfuse
docker compose down

# Stop and remove all data
docker compose down -v
```

## Access

- **Langfuse UI**: http://localhost:3003
- **MinIO Console**: http://localhost:9091 (admin: minioadmin/minioadmin)

## First-Time Setup

1. Open http://localhost:3003
2. Create an account (local auth, no email verification needed)
3. Create an organization
4. Create a project
5. Go to Project Settings → API Keys
6. Create new API keys

## Running Integration Tests

Set the API keys as environment variables:

```bash
export LANGFUSE_LOCAL_PUBLIC_KEY="pk-lf-..."
export LANGFUSE_LOCAL_SECRET_KEY="sk-lf-..."
export LANGFUSE_LOCAL_URL="http://localhost:3003"

# Run integration tests
cd ..
dotnet test --filter "Category=Integration&Environment=Local"
```

## Services

| Service | Port | Description |
|---------|------|-------------|
| langfuse-web | 3003 | Main Langfuse application |
| langfuse-worker | - | Background job processor |
| langfuse-postgres | 5432 | PostgreSQL database |
| langfuse-clickhouse | 8123/9000 | ClickHouse analytics |
| langfuse-redis | 6379 | Redis cache |
| langfuse-minio | 9000/9001 | S3-compatible blob storage |

## Troubleshooting

### Services not starting
```bash
# Check service status
docker compose ps

# View logs for a specific service
docker compose logs langfuse-web
```

### Reset everything
```bash
docker compose down -v
docker compose up -d
```

### Memory issues
Ensure Docker has at least 4GB of memory allocated (Docker Desktop → Settings → Resources).


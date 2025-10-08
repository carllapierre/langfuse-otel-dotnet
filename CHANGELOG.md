# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

Nothing

## [0.1.0] - 2025-10-07

### Added
- Initial alpha release
- Core Langfuse OTLP exporter functionality
- `AddLangfuseExporter()` extension method for OpenTelemetry TracerProviderBuilder
- Automatic configuration from environment variables (`LANGFUSE_PUBLIC_KEY`, `LANGFUSE_SECRET_KEY`, `LANGFUSE_BASE_URL`)
- Support for IConfiguration-based configuration
- Code-based configuration override option
- Automatic OTLP endpoint construction with Basic Auth
- Full support for Semantic Kernel GenAI instrumentation
- Sample application demonstrating Semantic Kernel integration


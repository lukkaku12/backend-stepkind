# StepKind Backend (ASP.NET Core + PostgreSQL + pgvector)

Backend Web API para gestionar tutoriales y ejecutar búsqueda semántica sobre chunks con embeddings.

## Estructura

- `Models/` entidades de dominio (`Tutorial`, `TutorialChunk`)
- `Data/` `StepKindDbContext`, factory de migraciones y proyección para búsqueda semántica
- `DTOs/` contratos de request/response
- `Repositories/` acceso a datos (`ITutorialRepository`, `TutorialRepository`)
- `Services/` lógica de negocio (chunking, embeddings, indexación, búsqueda semántica)
- `Controllers/` endpoints HTTP
- `Config/` opciones (`OpenAiOptions`)
- `Extensions/` registro de dependencias
- `Migrations/` migración inicial de referencia

## Notas de pgvector

1. La extensión se declara en EF con `HasPostgresExtension("vector")`.
2. El campo `Embedding` usa `vector(1536)`.
3. Se recomienda índice HNSW para cosine distance:

```sql
CREATE INDEX IF NOT EXISTS idx_tutorial_chunks_embedding_hnsw
ON "TutorialChunks" USING hnsw ("Embedding" vector_cosine_ops);
```

## Variables esperadas

- `ConnectionStrings__DefaultConnection`
- `OpenAI__ApiKey`
- `OpenAI__EmbeddingModel`

## Endpoints

- `POST /api/tutorials`
- `GET /api/tutorials`
- `GET /api/tutorials/{id}`
- `POST /api/tutorials/{id}/reindex`
- `POST /api/search/semantic`

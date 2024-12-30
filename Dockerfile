# Use a imagem oficial do PostgreSQL
FROM postgres:latest

# Define a senha do PostgreSQL
ENV POSTGRES_PASSWORD=root

# Expose a porta do PostgreSQL
EXPOSE 5432

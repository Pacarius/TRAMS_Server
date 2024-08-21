FROM mysql:9.0.1
ENV MYSQL_DATABASE=mydb
ENV MYSQL_ROOT_PASSWORD=vtccenter
ENV MYSQL_TCP_PORT=6969
COPY DB.sql /docker-entrypoint-initdb.d/Dump.sql

# RUN mysql --default-authentication-plugin=mysql_native_password


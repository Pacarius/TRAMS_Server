FROM mariadb:lts
ENV MARIADB_DATABASE=mydb
ENV MARIADB_ROOT_PASSWORD=vtccenter
# ENV MYSQL_TCP_PORT=6969
ENV MARIADB_AUTO_UPGRADE=1
COPY DB_2.sql /docker-entrypoint-initdb.d/Dump.sql

# RUN mysql --default-authentication-plugin=mysql_native_password


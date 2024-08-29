FROM mariadb:lts
ENV MARIADB_DATABASE=mydb
ENV event_scheduler=ON
ENV MARIADB_ROOT_PASSWORD=vtccenter
# ENV MYSQL_TCP_PORT=6969
ENV MARIADB_AUTO_UPGRADE=1
COPY DB_2.sql /docker-entrypoint-initdb.d/01.sql
COPY EVENT.sql /docker-entrypoint-initdb.d/02.sql
COPY MariaConf.cnf /etc/mysql/conf.d/MariaConf.cnf
RUN chmod 644 /etc/mysql/conf.d/MariaConf.cnf
# RUN mysql --default-authentication-plugin=mysql_native_password


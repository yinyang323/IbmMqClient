mkdir .\_tmp
copy com.ibm.mq.allclient-9.0.4.0.jar .\_tmp\com.ibm.mq.allclient-9.0.4.0.jar
copy key.snk .\_tmp\key.snk
cd .\_tmp
..\..\ikvm\ikvmc -target:library -sharedclassloader { com.ibm.mq.allclient-9.0.4.0.jar }
..\..\dotnet\ildasm com.ibm.mq.allclient-9.0.4.0.dll /out:com.ibm.mq.allclient-9.0.4.0.il
..\..\dotnet\ilasm com.ibm.mq.allclient-9.0.4.0.il /dll /key=..\key.snk
copy com.ibm.mq.allclient-9.0.4.0.dll ..\com.ibm.mq.allclient-9.0.4.0.dll
cd ..\
rmdir .\_tmp /Q /S

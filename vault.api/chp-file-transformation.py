import sys
from awsglue.transforms import *
from awsglue.utils import getResolvedOptions
from pyspark.context import SparkContext
from awsglue.context import GlueContext
from awsglue.job import Job
from awsglue import DynamicFrame

def sparkSqlQuery(glueContext, query, mapping, transformation_ctx) -> DynamicFrame:
    for alias, frame in mapping.items():
        frame.toDF().createOrReplaceTempView(alias)
    result = spark.sql(query)
    return DynamicFrame.fromDF(result, glueContext, transformation_ctx)
args = getResolvedOptions(sys.argv, ['JOB_NAME'])
sc = SparkContext()
glueContext = GlueContext(sc)
spark = glueContext.spark_session
job = Job(glueContext)
job.init(args['JOB_NAME'], args)

# Script generated for node chp-file-source
chpfilesource = glueContext.create_dynamic_frame.from_catalog(database="chp-catalog", table_name="chp-chp_file_source", transformation_ctx="chpfilesource")

# Script generated for node chp-file-transform
SqlQuery79 = '''
select id, description, "Nimesh" as Owner from myDataSource
'''
chpfiletransform = sparkSqlQuery(glueContext, query = SqlQuery79, mapping = {"myDataSource":chpfilesource}, transformation_ctx = "chpfiletransform")

# Script generated for node chp-file-destination
chpfiledestination = glueContext.write_dynamic_frame.from_options(frame=chpfiletransform, connection_type="s3", format="csv", format_options={"quoteChar": "\"", "withHeader": True, "separator": "|"}, connection_options={"path": "s3://chp-file-destination", "partitionKeys": []}, transformation_ctx="chpfiledestination")

job.commit()
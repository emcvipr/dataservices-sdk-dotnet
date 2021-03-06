/*
 * Copyright 2010-2013 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Amazon.DataPipeline.Model;
    using Amazon.Runtime.Internal.Transform;

    namespace Amazon.DataPipeline.Model.Internal.MarshallTransformations
    {
      /// <summary>
      /// ValidationWarningUnmarshaller
      /// </summary>
      internal class ValidationWarningUnmarshaller : IUnmarshaller<ValidationWarning, XmlUnmarshallerContext>, IUnmarshaller<ValidationWarning, JsonUnmarshallerContext>
      {
        ValidationWarning IUnmarshaller<ValidationWarning, XmlUnmarshallerContext>.Unmarshall(XmlUnmarshallerContext context)
        {
          throw new NotImplementedException();
        }

        public ValidationWarning Unmarshall(JsonUnmarshallerContext context)
        {
            ValidationWarning validationWarning = new ValidationWarning();
          validationWarning.Warnings = null;
                        
            int originalDepth = context.CurrentDepth;
            int targetDepth = originalDepth + 1;
            while (context.Read())
            {
                if ((context.IsKey) && (context.CurrentDepth == targetDepth))
                {
                context.Read();
                context.Read();
              
              if (context.TestExpression("Id", targetDepth))
              {
                validationWarning.Id = StringUnmarshaller.GetInstance().Unmarshall(context);
                continue;
              }
  
              if (context.TestExpression("Warnings", targetDepth))
              {
                validationWarning.Warnings = new List<String>();
                        StringUnmarshaller unmarshaller = StringUnmarshaller.GetInstance();
                while (context.Read())
                {
                  if ((context.IsArrayElement) && (context.CurrentDepth == targetDepth))
                  {
                     validationWarning.Warnings.Add(unmarshaller.Unmarshall(context));
                  }
                  else if (context.IsEndArray)
                  {
                    break;
                  }
                }
                continue;
              }
  
                }
                else if (context.IsEndElement && context.CurrentDepth <= originalDepth)
                {
                    return validationWarning;
                }
            }
          

            return validationWarning;
        }

        private static ValidationWarningUnmarshaller instance;
        public static ValidationWarningUnmarshaller GetInstance()
        {
            if (instance == null)
                instance = new ValidationWarningUnmarshaller();
            return instance;
        }
    }
}
  

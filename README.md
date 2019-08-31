 ![logo](https://github.com/joelromanpr/Zipper/blob/master/art/logo.png)  
 
 
 ## An portable class library under the .NET Standard  
 ### Zipping files does not have to be hard!   
 
 
 #### Zipper uses a configuration based aproach, here is its simplest form. 
 ```csharp  
var config = new Zipper.ZipConfiguration()
{
   Source = dirWithYourPdfs,
   Destination = destination,
   Extension = ".pdf",
};   
     
Zipper.Zip(config);     
            
```
 
#### Need to console log the files you just zipped?    
 ```csharp  
...
var zipFile = Zipper.Zip(config);     
foreach (var file in zipFile.ZipItems)
{
    Console.WriteLine($"Zipped: {file.Name}");
}
            
```


#### What if we need to move those big sneaky log files from one drive to another but must leave the last log file written to? (usually locked by the file system) We can set a custom SkipAmount during configuration for that!

 ```csharp  
  var config = new Zipper.ZipConfiguration()
{
    Source = randomDir,
    Destination = destination
    Extension = ".log",
    SkipAmount = 1
};

Zipper.Zip(config);     

 ```  
#### Do you want to preserve the earliest 3? You can specify the SkipStrategy and the SkipCount just for that!  


 ```csharp  
var config = new Zipper.ZipConfiguration()
{
   ...
   SkipAmount = 3,
   Strategy = Zipper.ZipConfiguration.SkipStrategy.START
};

Zipper.Zip(config);     

 ```

### There is still cool things todo! So feel free to submit a pull request! 


- CLI
- Windows Client
- Publish revisions NuGet automatically via Continous Integration    

### Dev Notes  
You will notice that zome methods are self describing like **Zipper.Zip()** thanks to this I saw no need to add summary comments to it :)

# Contacts:

- JR
  - [GitHub - JR](https://github.com/joelromanpr)
  - [Instagram](https://www.instagram.com/joelromanpr/)
  - [Twitter](https://twitter.com/joelromanpr_)  


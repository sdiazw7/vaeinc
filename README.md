VAE.CLI.Flags
=============

A lightweight and easy to use command line flags library with the following features.

* Typesafe
* No Reflection
* Allows custom flag parsing
* Includes standard and useful builtin flag types.
* Help text generation
* Won't pollute your libraries with unnecessary attribute dependencies.

Example
-------

    using VAE.CLI.Flags;
    
    namespace MyApp
    {
        public void Main(string[] args)
        {
            var parser = new Parser();
            var intFlag = parser.AddIntFlag("myintflag", 5, "An integer flag");
            var myCustomFlag = parser.Add<CustomThing>("myCustomFlag", new Val<Thing>(defaultThing,
                (s) => {
                    // custom parsing code here
                    return thing;
                }, "A custom Thing flag");
            parser.Parse(args);
        }
    }

It's as easy as that.

License
=======

Licensed under the http://www.apache.org/licenses/LICENSE-2.0 License.

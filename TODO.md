# TODO

The following feature are not yet implemented. Please contact Mikayla if you are interested in helping out.

## Features

* simple project system. go to definition should open target file in the context of the original root.
* toolbar for picking root evaluation context to use for current file
* switch between related files command
* snippets

## Xml

* formatter
* outlining tagger that's better than the one from textmate
* document outline tool window
* breadcrumbs bar

## Parsing:

* improve the background parser scheduling
* replace xbuild/monodevelop condition parser with one based on the expression parser
* keep parsed expressions in the AST instead of reparsing as needed
* switch the XDom to use a red-green tree like Roslyn
* implement some incremental parsing. it should be fairly straightforward to do it for a subset of insertions, especially without < chars

## Completion

* Improve implicit and explicit triggering for file path segments
* trigger intellisense on |, indexed against | separated comparands
* Completion for condition functions e.g. Exists
* use RID graph for RID completion
* context based filtering of disallowed/existing attributes and elements
* well known flavor GUIDs

## Validation

* Squiggle assignments to nonexistent task params
* Check for invalid chars in paths
* Warn on assigning wrongly typed expression to property or task arg
* condition comparand validation, including |-separated properties
* validate condition functions e.g. Exists
* RID validation
* NuGet package ID validation
* error when assigning values to reserved properties and metadata
* check type of expressions assigned to properties/metadata/task params
* item and property function names and arguments
* check metadata refs have sufficient context

## Type resolution

* Compute type of expression/subexpression when possible'
* Some basic coercion e.g. string+path = path
* Add logic to figure out context of unqualified metadata

## Analyzers

* DefaultItemExcludes assignment that doesn't include previous value
* Nonexistent property/item/metadata - heuristic based on usage
* Unnecessary package references

## Fixes

* remove property with default value
* rename TargetFramework<->TargetFrameworks as appropriate

## Schema & type system

* Publish json schema schema
* Classname type with metadata for which msbuild metadata/property points to the assembly
* Metadata for filename type with expected extensions
* Metadata for filename type with property to use as base directory
* Infer default values from <foo condition="$(foo)==''">default</foo>
* Infer type from assignment from known type and comparison to known type
* Infer bool type from bool literal assigment
* API to dump barebones schema from inferred symbols
* pull task definitions from current roslyn workspace, if any

## Tests:

Although there are hundred of unit tests covering the lower level details, there are few/no tests in the following areas:

* validation
* schema based completion
* completion for various value types
* function completion & resolution
* schema inference
* schema composition

## Misc

* In addition to brute forcing imports, resolve them using real MSBuild engine
* Completion of inline C#
* show default value of property/metadata/items in tooltips
* parameter info tooltip when completing values
* properly handle MSBuild escaping and XML entities
* better highlighting colors
* add documentation for task parameters
* fix some of the [FIXMEs](https://github.com/mhutch/MonoDevelop.MSBuildEditor/search?utf8=%E2%9C%93&q=fixme&type=)
* Fix MSBuildProjectExtensionsPath eval with nonstandard intermediate dir
* Go to definition on tasks
* Add Rename command
* Make expand/shrink selection work on MSBuild expressions
* Do some perf work, cache inferred schemas?
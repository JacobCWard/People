# &lt;juicy-tile-editor&gt;

`<juicy-tile-editor>` is a Polymer Element that adds an editor to control the setup of `<juicy-tile-list>`

## Demo

[Check it live!](http://juicy.github.io/juicy-tile-editor)

## Usage

1. Install the component using [Bower](http://bower.io/):

    ```sh
    $ bower install juicy-tile-editor --save
    ```

2. Import Web Components' polyfill:

    ```html
    <script src="http://cdnjs.cloudflare.com/ajax/libs/polymer/0.3.0/platform.js"></script>
    <script src="http://cdnjs.cloudflare.com/ajax/libs/polymer/0.3.0/polymer.js"></script>
    ```

3. Import Custom Element:

    ```html
    <link rel="import" href="bower_components/juicy-tile-list/src/juicy-tile-list.html">
    <link rel="import" href="bower_components/juicy-tile-editor/src/juicy-tile-editor.html">
    ```

4. Start using it!

    ```html
    <juicy-tile-editor selectionMode></juicy-tile-editor>
    <juicy-tile-list></juicy-tile-list>
    ```

## Options

Attribute                    | Options             | Default      | Description
---                          | ---                 | ---          | ---
`selectionMode`              | *Boolean*           |              | If present, the editor starts in selection mode

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D

## License

[MIT License](http://opensource.org/licenses/MIT)
<html>
<body>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet"
      integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous">

<div class="container">
    <table class="table">
        <tr>
            <td>
                id
            </td>
            <td>
                名前
            </td>
            <td>
                説明
            </td>
        </tr>

        @foreach($items as $item)
            <tr>
                <td>
                    {{$item['id']}}
                </td>
                <td>
                    {{$item['name']}}
                </td>
                <td>
                    {{$item['explanation']}}
                </td>
            </tr>
        @endforeach


    </table>
</div>


<form action="{{route('accounts.index')}}">
    <div class="d-flex justify-content-center">
        <input class="btn btn-secondary" type="submit" value="戻る">
    </div>
</form>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz"
        crossorigin="anonymous"></script>

</body>
</html>



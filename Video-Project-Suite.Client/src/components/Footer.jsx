
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

const Footer = () => {
    return (
        <Box sx={{ bgcolor: 'primary.main', color: 'white', p: 2, position: 'fixed', bottom: 0, width: '100%', marginLeft: -3 }}>

            <Typography variant="body2">
                © 2025 Video Project Suite. All rights reserved.
                <br />
            </Typography>
        </Box>
    );
}

export default Footer;
